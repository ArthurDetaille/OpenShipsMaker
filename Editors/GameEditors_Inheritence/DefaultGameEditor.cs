using UnityEngine;

public class DefaultGameEditor : GameEditor {
    public override void Call() {
        // Hide Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Close() {

    }

    public override void EditorLogic() {

    }
}