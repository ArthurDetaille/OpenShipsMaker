using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public void hide_loading_screen() {
        Image bkg = GetComponent<Image>();
        Text txt = GetComponentInChildren<Text>();
        
        LeanTween.value(gameObject, 1f, 0f, 0.3f).setDelay(1f).setOnUpdate((t) => {
            Color c = new Color(0f, 0f, 0f, t);
            bkg.color = c;
            txt.color = c;
        });
    }
}
