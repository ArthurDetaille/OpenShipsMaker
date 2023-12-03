using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_FPS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera fps_camera;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameEditorManager editorManager;

    [Header("Movements")]
    [SerializeField] private float force_amplitude;
    [SerializeField] private float sensitvity;
    [SerializeField] private float max_cam_angle;

    private Vector3 last_mouse_position;
    private Rigidbody player_rb;
    private bool is_flashlight_on = false;

    private void Awake() {
        player_rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        last_mouse_position = Input.mousePosition;
        editorManager.SetPlayerControllerReference(this);
    }

    // Update is called once per frame
    void Update()
    {
        // if (!PartsPlacementManager.self.is_paused()) HandleMovement();

        bool can_move = editorManager.GetCurrentEditor().GetGameEditorProfile().can_player_move;
        if (can_move) { HandleMovement(); }
    }

    public void ToggleFlashLight() { SetFlashLight(!is_flashlight_on); }   

    public void SetFlashLight(bool on) {
        is_flashlight_on = on;
        flashlight.SetActive(on);
    }

    public void HandleMovement() {
        // TODO : fix ultra high sensiblity in builds
        Vector3 deltaMouse = new Vector2(Input.GetAxis("look_up"), Input.GetAxis("look_right")) * Time.deltaTime;
        this.transform.RotateAround(this.transform.position, Vector3.up, deltaMouse.x * sensitvity);
        fps_camera.transform.RotateAround(fps_camera.transform.position, this.transform.right, -deltaMouse.y * sensitvity);

        flashlight.transform.rotation = fps_camera.transform.rotation;

        Vector3 force = Vector3.zero;

        force += this.transform.forward * Input.GetAxis("forward") * force_amplitude * Time.deltaTime;
        force += this.transform.right * Input.GetAxis("right") * force_amplitude * Time.deltaTime;
        force += this.transform.up * Input.GetAxis("up") * force_amplitude * Time.deltaTime;

        player_rb.AddForce(force);

        last_mouse_position = Input.mousePosition;
    }

    public Camera GetFPSCamera() { return this.fps_camera; }
}
