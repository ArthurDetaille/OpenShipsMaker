using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedPart : MonoBehaviour
{
    // VARIABLES
    [Header("Placed Part")]
    [SerializeField] private Part part;
    [SerializeField] public Transform build_origin;
    [SerializeField] protected Anchor[] anchors;

    [HideInInspector] private PlacedPart parent_part;
    [HideInInspector] private Anchor parent_anchor;

    // ENGINE FUNCTIONS
    private void Awake() {
        // At Awake set all anchors' parent to this PlacedPart
        foreach (Anchor anchor in anchors) { anchor.set_parent_part(this); }
    }

    // SAVE MANAGMENT
    // format being "one{;;two:2{three:0{};three:1{}};;}"
    protected string SavedAttached() {
        // Attention : C'est très moche mais ça marche...
        string proprietes = '(' + BuildProprietesString() + ')';
        
        if (part.sid == "" || part.sid == null) {
            Application.Quit();
            Debug.LogError("stringId is null or empty for this part!");
        }

        string data = part.sid + proprietes + "{";
        
        for (int i = 0; i < anchors.Length; i++) {
            Anchor anchor = anchors[i];
            string separator = (i == anchors.Length - 1) ? "" : ";";
            string part_string = (anchor.GetChildPart() == null) ? "" : anchor.GetChildPart().SavedAttached();
            data += part_string + separator;
        }

        return data + "}";
    }

    // Cette fonction est à surcharger pour chaque surcharge de PlacedPart
    // (si nécessaire)
    public virtual string BuildProprietesString() {
        string props = "";
        if (part.can_rotate) props += "rotation=" + parent_anchor.GetRotation();

        return props;
    }

    // DESTROY
    public void DestroyChilds() {
        foreach(Anchor anchor in anchors) { anchor.DestroyChild(); }
    }

    // ABSTRACT PROPRIETES
    public virtual void SetFromProprietes(SaveAnchorResult result) {}


    // GETTERS AND SETTERS
    public void         SetParentPart(PlacedPart part) { parent_part = part; }
    public PlacedPart   GetParentPart() { return parent_part; }
    
    public void         SetParentAnchor(Anchor anchor) { parent_anchor = anchor; }
    public Anchor       GetParentAnchor() { return parent_anchor; }
    
    public Part         GetPartObject() { return this.part; }
    public Anchor[]     GetAnchors() { return anchors; }
}
