using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [HideInInspector] private RectTransform pausePanel;

    private bool dummy_pause = true;
    
    public void ApplicationQuit() { Application.Quit(); }

    public void toggle_pause_menu(bool paused) {
        float start = paused ? 0f : 1f;
        LeanTween.value(pausePanel.gameObject, start, 1f - start, 0.2f).setOnUpdate((value => {
            pausePanel.localScale = new Vector2(value, value);
        }));
    }

    // ENGINE FUNCTIONS
    private void Awake() {
        pausePanel = this.GetComponent<RectTransform>();    
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) { dummy_pause = !dummy_pause; toggle_pause_menu(dummy_pause); } 
    }
}
