using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsPlacementManager : MonoBehaviour
{
    // VISIBLE VARIABLES
    [Header("Pointing options")]
    [SerializeField] private float max_distance_placement;
    [SerializeField] private Camera fps_camera;

    [Header("Ghost part")]
    [SerializeField] private Material ghost_material;
    [SerializeField] private Color ghost_wrong_color;
    [SerializeField] private Color ghost_right_color;

    [Header("Manager References")]
    [SerializeField] private SelectionBarManager selectionBarManager;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private PauseMenuManager pauseManager;

    [Header("Parts")]
    [SerializeField] public Part[] parts;
    [SerializeField] private int default_selected_part = 0;

    // HIDDEN VARIABLES
    [HideInInspector] private GameObject ghost_part;
    [HideInInspector] private bool build_mode = false;
    [HideInInspector] private bool paused = false;
    [HideInInspector] public int currently_selected = 0;

    public static PartsPlacementManager self;
    
    // ENGINE FUNCTIONS
    private void Awake() {
        Cursor.visible = false;
        self = this;
        Cursor.lockState = CursorLockMode.Locked;
        
        SaveSystem.instance.SetPartsPool(parts);
    }

    void Start() {
        currently_selected = default_selected_part;
        CreateGhostPart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) { ToggleBuildMode(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { SetPaused(!paused); }

        if (build_mode && !paused) raycast();

        // Handle change of parts via scroll wheel
        float scroll = Input.GetAxis("scroll_up");
        if (scroll != 0) {
            currently_selected += scroll > 0 ? -1 : 1;
            if (currently_selected >= parts.Length) currently_selected = 0;
            if (currently_selected < 0) currently_selected = parts.Length - 1;
            
            HandlePartChange();
        }

        // TODO : Handle change of parts via numerals
    }

    private void HandlePartChange() {
        // destroy old ghost and create the new one
        // then animate the selection bar to display the right part

        CreateGhostPart();
        selectionBarManager.SelectCurrentPart();
    }

    // MANAGE GHOST PART    
    private void CreateGhostPart() {
        // this function handles the creation and the destruction of the ghost
        // used in-game to visualise a piece we would like to place
        
        // 1 - Destroy the existing one (if it exists) and create the new one
        if (ghost_part) {
            GameObject temp = ghost_part;
            Destroy(temp);
        }

        GameObject prefab = parts[currently_selected].prefab;

        ghost_part = Instantiate(prefab) as GameObject;
        ghost_part.SetActive(build_mode);

        // 2 - it destroy useless components attached to it (optimisation)
        Destroy(ghost_part.GetComponent<PlacedPart>());

        // 3 - Applies the correct material
        foreach (MeshRenderer mr in ghost_part.GetComponentsInChildren<MeshRenderer>()) {
            mr.material = ghost_material;
            mr.material.color = ghost_wrong_color;
            mr.receiveShadows = false;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        // 4 - Destroy colliders (prevent additional useless computations and interference with raycast)
        foreach(Collider collider in ghost_part.GetComponentsInChildren<Collider>()) Destroy(collider);

        // 5 - Tweens it, so it looks nice !
        ghost_part.transform.localScale = Vector3.zero;
        LeanTween.scale(ghost_part, Vector3.one, .1f);
    }

    private void SetGhostPartPlacable(bool placable) {
        // put the correct material onto the ghost
        // green for buildable part, red for non-buildable part

        foreach (MeshRenderer mr in ghost_part.GetComponentsInChildren<MeshRenderer>()) {
            mr.material.color = placable ? ghost_right_color : ghost_wrong_color;
        }
    }

    // RAYCAST
    private void raycast() {
        RaycastHit hit;
        Anchor anchor = null;

        bool has_hit = Physics.Raycast(
            fps_camera.transform.position + fps_camera.transform.forward * 0.5f,
            fps_camera.transform.forward,
            out hit,
            max_distance_placement
        );

        float distance = max_distance_placement;
        if (has_hit) { anchor = hit.collider.gameObject.GetComponent<Anchor>(); distance = hit.distance; }

        SetGhostPartPlacable(anchor != null);

        ghost_part.transform.position = fps_camera.transform.position + fps_camera.transform.forward * distance;
        // if (anchor) 

        // if (Input.GetMouseButtonDown(0)) {
        //     if (anchor != null) {
        //         Part part_to_place = parts[currently_selected];
        //         anchor.attach_part(part_to_place);
        //     } else {
        //         popupManager.display_popup("You can't place there.", 0.0f, PopupStyle.Fast);
        //     }
        // }


        if (anchor) {
            anchor.SnapGhost(ghost_part);
            
            // handle placement
            if (Input.GetMouseButtonDown(0)) {
                Part part_to_place = parts[currently_selected];
                anchor.attach_part(part_to_place);
            }

            // handle rotation
            if (Input.GetKeyDown(KeyCode.R) && parts[currently_selected].can_rotate) {
                anchor.increment_rotation(ghost_part);
            }
        }
    }

    // MANAGE SET AND TOGGLE MODES
    public void SetPaused(bool pause) {
        SetBuildMode(false);
        paused = pause;
        pauseManager.toggle_pause_menu(paused);
        Cursor.visible = pause;

        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePaused() {
        SetBuildMode(false);
        SetPaused(!paused);
    }

    public void SetBuildMode(bool set) {
        ghost_part.SetActive(set);
        selectionBarManager.SetBuildMode(set);
    }

    private void ToggleBuildMode() {
        build_mode = !build_mode;
        SetBuildMode(build_mode);
    }

    // GETTERS AND SETTERS
    public bool is_paused() { return paused; }
    public bool is_building() { return build_mode; }
}
