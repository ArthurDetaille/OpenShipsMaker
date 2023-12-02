using UnityEngine;

public class EditorManager : MonoBehaviour {
    public Editor[] editors;
    public PlayerController_FPS playerController;

    public Editor pause_editor;
    public Editor active_editor;

    public void Update() {
        // check for change
        if (Input.GetKeyDown(KeyCode.Escape)) { SwitchEditor(pause_editor); return; }

        for (int i = 0; i < editors.Length; i++) {
            if (Input.GetKeyDown(editors[i].call_keyCode)) {
                SwitchEditor(editors[i]);
                return;
            }
        }
    }

    public void SwitchEditor(Editor editor_to_call) {
        active_editor.Close();
        editor_to_call.Call();

        active_editor = editor_to_call;

        active_editor.EditorLogic();
    }
}