using UnityEngine;

public class GameEditorManager : MonoBehaviour {
    [HideInInspector]   private GameEditor default_editor;
    [HideInInspector]   private GameEditor active_editor;
    [HideInInspector]   private GameEditor pause_editor;
    [HideInInspector]   private GameEditor last_active_editor;
    [HideInInspector]   private GameEditor[] editors;

    [HideInInspector]   private PlayerController_FPS playerController;


    // ENGINE FUNCTIONS
    public void Awake() {
        default_editor = this.GetComponent<DefaultGameEditor>();
        pause_editor = this.GetComponent<PauseGameEditor>();
        editors = this.GetComponents<GameEditor>();

        active_editor = default_editor;
        active_editor.Call();

        foreach (GameEditor e in editors) { e.SetManager(this); }
    }

    public void Update() {
        foreach (GameEditor editor in editors) {
            if (!editor.GetCallable()) continue;
            
            if (Input.GetKeyDown(editor.GetGameEditorProfile().call_keyCode)) {
                if (editor == active_editor) {
                    SetEditor(default_editor);
                    break;
                }

                SwitchEditor(editor);
            }
        }

        active_editor.EditorLogic();
    }

    // EDITORS MANAGMENT
    public void SwitchEditor(GameEditor editor_to_call) {        
        Debug.Log($"Switch game editor {active_editor.GetName()} -> {editor_to_call.GetName()}");

        last_active_editor = active_editor;
        active_editor = editor_to_call;

        last_active_editor.Close();
        editor_to_call.Call();
        editor_to_call.EditorLogic();
    }

    public void SetEditor(GameEditor e) {
        if (e == active_editor) return;

        active_editor.Close();
        last_active_editor = active_editor;
        active_editor = e;

        last_active_editor.Close();
        active_editor.Call();
    }

    // GETTERS AND SETTERS
    public GameEditor   GetCurrentEditor()  { return active_editor; }
    public GameEditor[] GetEditorPool()     { return editors; }

    public void SetPlayerControllerReference(PlayerController_FPS controller) {
        this.playerController = controller;
        foreach (GameEditor e in editors) { e.SetPlayerControllerReference(controller); }
    }

    public PlayerController_FPS GetPlayerControllerReference() { return this.playerController; }
    public GameEditor GetLastActiveEditor() { return this.last_active_editor; }
}