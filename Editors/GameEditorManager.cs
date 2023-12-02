using UnityEngine;

public class GameEditorManager : MonoBehaviour {
    public GameEditor[] editors;
    public PlayerController_FPS playerController;

    public GameEditor pause_editor;
    public GameEditor active_editor;

    public void Update() {
        // check for change
        if (Input.GetKeyDown(KeyCode.Escape)) { SwitchEditor(pause_editor); return; }

        for (int i = 0; i < editors.Length; i++) {
            GameEditorProfile profile = editors[i].GetGameEditorProfile();
            if (Input.GetKeyDown(profile.call_keyCode)) {
                SwitchEditor(editors[i]);
                return;
            }
        }
    }

    public void SwitchEditor(GameEditor editor_to_call) {
        active_editor.Close();
        editor_to_call.Call();

        active_editor = editor_to_call;

        active_editor.EditorLogic();
    }
}