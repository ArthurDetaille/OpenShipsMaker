using UnityEngine;

[System.Serializable]
public struct EditorProfile {
    public string name;
    public bool can_player_move;
    public KeyCode call_keyCode;
}

public class Editor : MonoBehaviour {
    protected abstract EditorProfile profile;

    protected abstract void Call();
    protected abstract void Close();

    protected abstract void EditorLogic();

    public EditorProfile GetEditorProfile() { return profile; }
}