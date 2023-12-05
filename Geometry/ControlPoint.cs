using System;
using UnityEngine;

[System.Serializable]
public enum GizmoType {
    NONE,
    TRANSLATE_FORWARD,
    TRANSLATE_ALL_DIRECTION,
    TRANSLATE_PERPENDICULAR
}

[System.Serializable]
public class ControlPoint : MonoBehaviour {
    public GizmoType gizmo_type;
    public string identification_string;

    // Getters and setters
    public void SetPosition(Vector3 position)           { this.transform.position = position; }
    public void SetPositionFromTransform(Transform t)   { this.transform.position = t.position; }
    
    // Display function
    public void OnStartHoverGizmo() { 
        // TODO : tween
    }

    public void OnEndHoverGizmo() {}

    public void HandleControlPointMovement() {}
}