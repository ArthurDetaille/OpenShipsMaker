using UnityEngine;

[System.Serializable]
public struct GameEditorProfile {
    public string name;
    public bool can_player_move;
    public KeyCode call_keyCode;
}

public abstract class GameEditor : MonoBehaviour {
    [SerializeField] protected GameEditorProfile profile;

    public abstract void Call();
    public abstract void Close();

    public abstract void EditorLogic();

    public GameEditorProfile GetGameEditorProfile() { return profile; }
}