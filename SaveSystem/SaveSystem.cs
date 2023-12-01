using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

public struct SaveAnchorResult {
    // cette struct est la preuve que je suis le Hitler du jeu vidéo
    public Part part;
    public string[] anchors_content;
    public Dictionary<string, float> proprieties;

    public SaveAnchorResult(Part part, string[] anchors_content, Dictionary<string, float> props) {
        this.part = part;
        this.anchors_content = anchors_content;
        this.proprieties = props;
    }

    // l'idée c'est dans le cas d'une ancre vide, on retourne un anchors_content null
    // Cela fonctionne puisque en temps normal elle est de taille 32
    // et donc, le fait qu'elle soit null représente une anomalie
    // que l'on qualifie donc d'ancre vide
}

public struct BuildAnimationProfile {
    public bool animate;
    public float duration;
    public float delay;

    public BuildAnimationProfile(bool animate = false, float duration = 0.3f, float delay = 0f) {
        this.animate = animate;
        this.duration = duration;
        this.delay = delay;
    }
}

public enum AccumulatorType {
    STAND_BY,
    STRING_ID,
    PROPRIETES,
    FLUSH_ANCHORS,
    ANCHORS
}

public class SaveSystem : MonoBehaviour {
    // Si je fais les choses correctement (c'est-à-dire que le système est assez général)
    // je serais en mesure d'implémenter un system de blueprints
    // autrement dit des bouts entiers de vaisseaux que l'on peut "copier-coller"

    // VARIABLES (HIDDEN)
    public static SaveSystem instance;
    private Part[] parts;
    private Dictionary<string, Part> parts_stringID_reference;

    public static int MAX_ANCHORS_LIMIT = 32;

    // ENGINE FUNCTIONS
    private void Awake() { instance = this; }
    private void Start() { BuildStringIDReferenceDictionary(); }

    
    // SAVE SYSTEM FUNCTIONS
    public SaveAnchorResult ParseAnchorContentFromString(string content, bool getPart) {
        if (content == null || content == "") return new SaveAnchorResult(parts[0], null, null);

        AccumulatorType accumulatorType = AccumulatorType.STRING_ID;

        int count = 0;
        int string_length = content.Length;
        string anchors = "";
        string props = "";
        string stringID = "";

        int next_anchor_ptr = 0;
        
        // This code fixes a limit of 'MAX_ANCHORS_LIMIT = 32' to the number of anchors on a Part
        string[] anchors_content = new string[MAX_ANCHORS_LIMIT];

        for (int i = 0 ; i < string_length; i++) {
            char curr = content[i];
            bool skip = false;

            switch (curr) {
                case ';' :
                    if (count == 1) accumulatorType = AccumulatorType.FLUSH_ANCHORS;
                    break;

                case '{' :
                    count++;
                    accumulatorType = AccumulatorType.ANCHORS;
                    if(count == 1) skip = true;
                    break;

                case '}' :
                    count--;
                    accumulatorType = AccumulatorType.ANCHORS;
                    if(count == 0) accumulatorType = AccumulatorType.FLUSH_ANCHORS;
                    break;

                case '(' :
                    if (count >= 1) break;
                    accumulatorType = AccumulatorType.PROPRIETES;
                    skip = true;
                    break;

                case ')' :
                    if (count >= 1) break;
                    accumulatorType = AccumulatorType.STAND_BY;
                    skip = true;
                    break;
            }

            if (skip) continue;

            switch (accumulatorType) {
                case AccumulatorType.STAND_BY :
                    break;

                case AccumulatorType.STRING_ID :
                    stringID += curr;
                    break;

                case AccumulatorType.PROPRIETES :
                    props += curr;
                    break;

                case AccumulatorType.ANCHORS :
                    anchors += curr;
                    break;

                case AccumulatorType.FLUSH_ANCHORS :
                    anchors_content[next_anchor_ptr] = anchors;
                    next_anchor_ptr ++;
                    anchors = "";
                    accumulatorType = AccumulatorType.ANCHORS;
                    break;
            }
        }

        if (!getPart) return new SaveAnchorResult(parts[0], anchors_content, null);

        Dictionary<string, float> proprieties = ParseProprietesFromString(props);
        return new SaveAnchorResult(parts_stringID_reference[stringID], anchors_content, proprieties);
    }

    public Dictionary<string, float> ParseProprietesFromString(string proprietes) {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        string[] tokens = proprietes.Split(",");
        
        foreach(string token in tokens) {
            string[] values = token.Split('=');
            dict.Add(values[0], float.Parse(values[1]));
        }
        
        return dict;
    }


    // UTILITIES
    private void BuildStringIDReferenceDictionary() {
        if (parts == null) Debug.LogError("Parts are null in SaveSystem");
        parts_stringID_reference = new Dictionary<string, Part>();
        
        foreach(Part part in parts) {
            if (part.sid == "") continue;

            parts_stringID_reference.Add(part.sid, part);
        }
    }


    // GETTERS AND SETTERS
    // for an exterior script to set parts : centralization's sake
    public void SetPartsPool(Part[] parts) { this.parts = parts; }
    public Part[] GetPartsPool() { return parts; }
}