using UnityEngine;
using UnityEngine.UI;

public class PauseGameEditor : GameEditor {
    public GameObject UI_panel;

    public static float ANIMATION_DURATION = 0.2f;

    public override void Call() {
        UI_panel.SetActive(true);
        LeanTween.value(UI_panel, 0f, 1f, ANIMATION_DURATION).setOnUpdate((t) => {
            UI_panel.transform.localScale = Vector3.one * t;
        });

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Close() {
        UI_panel.SetActive(false);
        LeanTween.value(UI_panel, 1f, 0f, ANIMATION_DURATION).setOnUpdate((t) => {
            UI_panel.transform.localScale = Vector3.one * t;
        });

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void EditorLogic() {

    }

    public void UIButtonResume() {
        this.manager.SwitchEditor(this.manager.GetLastActiveEditor());
    }

    public void UIButtonQuit() {
        Application.Quit();
    }

    public void UILaunchShip() {

    }
}