using UnityEngine;

public class SceneManager : MonoBehaviour {  
    public void LoadScene(int index) {
        PlayerPrefs.SetInt("active_scene_index", index);
    }
}