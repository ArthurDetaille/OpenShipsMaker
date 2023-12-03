using UnityEngine;

public class PlacementGameEditor : GameEditor {    
    [SerializeField] private GameObject flashlight;

    [Header("Ghost part")]
    [SerializeField]    private Material ghost_material;
    [SerializeField]    private Color ghost_wrong_color;
    [SerializeField]    private Color ghost_right_color;

    [Header("Manager References")]
    [SerializeField]    private SelectionBarManager selectionBarManager;
    [SerializeField]    private PopupManager popupManager;
    [SerializeField]    private SaveSystem saveSystem; 

    [Header("Parts")]
    [SerializeField]    public Part[] parts;

    // HIDDEN VARIABLES
    [HideInInspector]   private int currently_selected = 0;
    [HideInInspector]   private GameObject ghost_part;

    // STATIC VARIABLES
    private static float MAX_DISTANCE_PLACEMENT = 6.0f;

    // ENGINE FUNCTIONS
    private void Awake() {
        selectionBarManager.SetPlacementGameEditorReference(this);
        saveSystem.SetPartsPool(parts);
    }

    // SURCHARGE
    public override void Call() {
        CreateGhostPart();
        selectionBarManager.SetVisibility(true);
    }

    public override void Close() {
        HideGhostPart();
        selectionBarManager.SetVisibility(false);
    }

    public override void EditorLogic() {
        if (Input.GetKeyDown(KeyCode.F)) { ToggleFlashLight(); }

        RayCast();

        // Handle change of parts via scroll wheel
        float scroll = Input.GetAxis("scroll_up");
        if (scroll != 0) {
            currently_selected += scroll > 0 ? -1 : 1;
            if (currently_selected >= GetPartsCount())  currently_selected = 0;
            if (currently_selected < 0)                 currently_selected = GetPartsCount() - 1;
            
            HandlePartChange();
        }
    }

    // LOGIC
    private void RayCast() {
        RaycastHit hit;
        Anchor anchor = null;

        Camera cam = playerController.GetFPSCamera();

        bool has_hit = Physics.Raycast(
            cam.transform.position + cam.transform.forward * 0.5f,
            cam.transform.forward,
            out hit,
            MAX_DISTANCE_PLACEMENT
        );

        float distance = MAX_DISTANCE_PLACEMENT;
        if (has_hit) { anchor = hit.collider.gameObject.GetComponent<Anchor>(); distance = hit.distance; }

        SetGhostPartPlacable(anchor != null);

        ghost_part.transform.position = cam.transform.position + cam.transform.forward * distance;

        if (anchor) {
            anchor.SnapGhost(ghost_part);
            
            // handle placement
            if (Input.GetMouseButtonDown(0)) {
                Part part_to_place = GetCurrentPart();
                anchor.attach_part(part_to_place);
            }

            // handle rotation
            if (Input.GetKeyDown(KeyCode.R) && GetCurrentPart().can_rotate) {
                anchor.increment_rotation(ghost_part);
            }
        }
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
        ghost_part.SetActive(true);

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

    private void HideGhostPart() {
        ghost_part.SetActive(false);
    }

    private void SetGhostPartPlacable(bool placable) {
        // put the correct material onto the ghost
        // green for buildable part, red for non-buildable part

        foreach (MeshRenderer mr in ghost_part.GetComponentsInChildren<MeshRenderer>()) {
            mr.material.color = placable ? ghost_right_color : ghost_wrong_color;
        }
    }

    private void HandlePartChange() {
        // destroy old ghost and create the new one
        // then animate the selection bar to display the right part

        CreateGhostPart();
        selectionBarManager.SelectCurrentPart();
    }

    // GAMEPLAY
    public void ToggleFlashLight() { flashlight.SetActive(!flashlight.activeSelf); }

    // GETTERS & SETTERS
    public Part GetCurrentPart() { return parts[currently_selected]; }
    public int GetPartsCount() { return parts.Length; }
    public int GetCurrentPartSelectionIndex() { return currently_selected; }
    public Part GetPartAtIndex(int i) { return parts[i]; }
}