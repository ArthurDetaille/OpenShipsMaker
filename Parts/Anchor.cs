using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    // VARIABLES
    [SerializeField]    private PlacedPart parent_part;
    [HideInInspector]   private PlacedPart child_part = null;

    // c'est rempli de cross-referencing
    // tout simplement parceque j'ai peur de faire du refactoring
    // si je dois aller en enfer c'est pour ce spaghetti code

    private float rotation = 0f;

    // ENGINE FUNCTIONS
    private void Start() {
        // just making sure there is aways a PlacedPart set as parent_part
        // I don't want my game to explose
        // it's executing after awake just in order for it's parent placed part to wake up
        // and set all of it's anchors' parent_part (this one included)
        if (parent_part == null) { parent_part = this.transform.parent.GetComponent<PlacedPart>(); }
    }

    // PARTS
    public void attach_part(Part part) {
        if (parent_part == null) {
            Debug.LogError("NULL REFERENCE : parent_part is null !");
            return;
        }

        GameObject part_go = Instantiate(part.prefab) as GameObject;
        SetTransform(part_go);
        part_go.transform.SetParent(this.transform);

        PlacedPart placed_part = part_go.GetComponent<PlacedPart>();
        placed_part.SetParentPart(parent_part);
        placed_part.SetParentAnchor(this);
        child_part = placed_part;
    }

    public void SnapGhost(GameObject ghost) {
        SetTransform(ghost);
    }

    public void DestroyChild() {
        if (child_part == null) return;

        Anchor[] child_anchors = child_part.GetAnchors();
        foreach (Anchor anchor in child_anchors) { anchor.DestroyChild(); }
        Destroy(child_part.gameObject);
    }

    // A common function to SnapGhost and attach_parent
    // so i can change everything in the same place
    // in order for the ghost part and the actual placed part to look the same
    // ... and faster iterations
    private void SetTransform(GameObject go) {
        go.transform.position = this.transform.position;
        go.transform.rotation = Quaternion.LookRotation(this.transform.forward);
        go.transform.RotateAround(this.transform.forward, this.rotation * Mathf.PI / 2f);
    }

    public void increment_rotation(GameObject go) {
        this.rotation += 1f;
        SetTransform(go);
    }

    // SAVE MANAGER
    public void BuildPartFromString(SaveAnchorResult result) {
        if (result.anchors_content == null) return;
        attach_part(result.part);
        this.GetChildPart().SetFromProprietes(result);

        if (result.part.can_rotate) {
            float rot = float.Parse(result.properties["rotation"]);
            this.transform.RotateAround(this.transform.forward, rot * Mathf.PI / 2f);
        }

        Anchor[] part_anchors = child_part.GetAnchors();
        int anchors_count = part_anchors.Length;

        for (int i = 0; i < anchors_count; i++) {
            string content = result.anchors_content[i];
            SaveAnchorResult r = SaveSystem.instance.ParseAnchorContentFromString(content, true);
            part_anchors[i].BuildPartFromString(r);
        }
    }

    // GETTERS AND SETTERS
    public void         SetChildPart(PlacedPart part) { child_part = part; }
    public PlacedPart   GetChildPart() { return child_part; }

    public void         set_parent_part(PlacedPart part) { parent_part = part; }
    public PlacedPart   GetParentPart() { return parent_part; }

    public float        GetRotation() { return this.rotation; }
}