using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotCameraController : MonoBehaviour
{
    [SerializeField]    private float sensibility;
    [SerializeField]    private float scroll_sensibility;
    [SerializeField]    private float distance;
    [SerializeField]    private GameEditorManager editorManager;

    private static float MAX_CAMERA_DISTANCE = 50.0f;
    private static float MIN_CAMERA_DISTANCE = 5.0f;

    private Camera camera;

    private void Awake() { camera = this.GetComponentInChildren<Camera>(); }

    // Update is called once per frame
    void Update()
    {
        if (editorManager.GetCurrentEditor().GetGameEditorProfile().can_player_move) HandleMovement();
    }

    private void HandleMovement() {
        this.transform.RotateAround(Vector3.up, Input.GetAxis("look_right") * sensibility * Time.deltaTime);  
        this.transform.RotateAround(camera.transform.right, -Input.GetAxis("look_up") * sensibility * Time.deltaTime);


        float scroll_delta = Input.GetAxis("scroll_up") * scroll_sensibility * Time.deltaTime;

        if (Mathf.Abs(scroll_delta) >= 0.01f) {
            float raw_new_distance = distance - scroll_delta;
            float new_distance = Mathf.Clamp(raw_new_distance, MIN_CAMERA_DISTANCE, MAX_CAMERA_DISTANCE);
            
            LeanTween.value(camera.gameObject, distance, new_distance, 0.1f).setOnUpdate((t) => {
                distance = t;
                camera.transform.localPosition = -Vector3.forward * distance;
            });
        }
    }
}
