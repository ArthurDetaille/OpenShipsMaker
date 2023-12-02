using UnityEngine;

public class GameEditorManager : MonoBehaviour {
    public PlayerController_FPS playerController;

    [HideInInspector]   public GameEditor pause_editor;
    [HideInInspector]   public GameEditor active_editor;
    [HideInInspector]   public GameEditor[] editors;

    public void Awake() {
        editors = this.GetComponents<GameEditor>();
        pause_editor = this.GetComponent<PauseGameEditor>();
    }

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