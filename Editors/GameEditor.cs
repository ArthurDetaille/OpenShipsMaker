using UnityEngine;

[System.Serializable]
public struct GameEditorProfile {
    public string name;
    public bool can_player_move;
    public bool callable;
    public KeyCode call_keyCode;
}

public abstract class GameEditor : MonoBehaviour {
    [SerializeField]    protected GameEditorProfile profile;
    [HideInInspector]   protected PlayerController_FPS playerController;
    [HideInInspector]   protected GameEditorManager manager;

    public abstract void Call();
    public abstract void Close();

    public abstract void EditorLogic();

    public GameEditorProfile GetGameEditorProfile() { return profile; }
    public bool GetCallable() { return profile.callable; }
    public string GetName() { return profile.name; }
    public void SetPlayerControllerReference(PlayerController_FPS controller) { this.playerController = controller; }
    public void SetManager(GameEditorManager m) { this.manager = m; }
}