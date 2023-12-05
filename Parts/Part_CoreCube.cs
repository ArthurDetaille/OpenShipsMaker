using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Part_CoreCube : PlacedPart
{
    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) { PasteAndLoadFromClipBoard(); }
        if (Input.GetKeyDown(KeyCode.C)) { LoadAndCopyToClipboard(); }
        if (Input.GetKeyDown(KeyCode.X)) { DestroyCurrentShip(); }
    }

    public override string BuildProprietesString() {
        return "name=ships";
    }

    private void PasteAndLoadFromClipBoard() {
        string clipBoard = GUIUtility.systemCopyBuffer;
        Debug.Log(clipBoard);

        if (clipBoard == "" || clipBoard == null) {
            PopupManager.instance.display_popup("Your clipboard is empty!", 0f, PopupStyle.Medium);
            return;
        }

        SaveAnchorResult result = SaveSystem.instance.ParseAnchorContentFromString(clipBoard, false);
        for (int i = 0; i < anchors.Length; i++) {
            string c = result.anchors_content[i];
            SaveAnchorResult r = SaveSystem.instance.ParseAnchorContentFromString(c, true);
            anchors[i].BuildPartFromString(r);
        }

        ShipPhysicsManager phManager = this.GetComponent<ShipPhysicsManager>();
        if (phManager != null) phManager.UpdateMass(); 

        PopupManager.instance.display_popup("Loaded a ship from clipBoard!", 0f, PopupStyle.Medium);
    }

    private void LoadAndCopyToClipboard() {
        string content = SavedAttached();
        GUIUtility.systemCopyBuffer = content;

        PopupManager.instance.display_popup("Saved a ship to clipBoard!", 0f, PopupStyle.Medium);
    }

    private void DestroyCurrentShip() {
        PopupManager.instance.display_popup("Deleted ship", 0f, PopupStyle.Medium);
        DestroyChilds();
    }
}