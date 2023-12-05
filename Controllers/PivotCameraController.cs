using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotCameraController : MonoBehaviour
{
    [SerializeField]    private float sensibility;
    [SerializeField]    private float distance;
    [SerializeField]    private GameEditorManager editorManager;

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
    }
}
