using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopupStyle {
    Fast,
    Medium,
    Long
}

public class PopupManager : MonoBehaviour
{
    [SerializeField] private Text popup_text;

    public static PopupManager instance;

    private void Awake() {
        instance = this;
    }

    public void display_popup(string text, float delay, PopupStyle style) {
        popup_text.gameObject.SetActive(true);

        LeanTween.value(gameObject, 0f, 1f, 0.2f).setDelay(delay).setOnUpdate( (value) => {
            popup_text.text = text;
            popup_text.color = new Color(1f, 1f, 1f, value);
        });

        float span = 1.0f;
        if (style == PopupStyle.Fast) span = 0.8f;
        if (style == PopupStyle.Medium) span = 1.5f;
        if (style == PopupStyle.Long) span = 2.5f;

        LeanTween.value(gameObject, 1f, 0f, 0.2f).setDelay(delay + span).setOnUpdate( (value) => {
            popup_text.color = new Color(1f, 1f, 1f, value);
        });
    }

    private void Start() {
        display_popup("Press <Tab> to switch to build mode.", 1.0f, PopupStyle.Medium);
        display_popup("Use Z,Q,S & D to move around.", 3.5f, PopupStyle.Medium);
        display_popup("Press <C> to save a ship to clipboard and <V> to load a ship from it.", 6f, PopupStyle.Long);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) display_popup("Press <Tab> to switch to build mode.", 0.0f, PopupStyle.Medium);
    }
}
