using System;
using UnityEngine;

public enum GizmoType {
    NONE,
    TRANSLATE_FORWARD
}

[System.Serializable]
public struct ControlPoint {
    public Vector3 position;
    public GizmoType[] gizmo_types;
    public GameObject gizmo_model;

    public ControlPoint(Vector3 pos, GizmoType[] gType) {
        this.position = pos;
        this.gizmo_types = gType;
        this.gizmo_model = null;
    }

    public ControlPoint(Vector3 pos, GizmoType gType) {
        this.position = pos;
        this.gizmo_types = new GizmoType[] {gType};
        this.gizmo_model = null;
    }

    public ControlPoint(GizmoType[] gType) {
        this.position = Vector3.forward;
        this.gizmo_types = gType;
        this.gizmo_model = null;
    }

    public ControlPoint(GizmoType gType) {
        this.position = Vector3.forward;
        this.gizmo_types = new GizmoType[] {gType};
        this.gizmo_model = null;
    }

    // Getters and setters
    public void SetGizmoModelReference(GameObject ref_) { this.gizmo_model = ref_; }
    public void SetPosition(Vector3 position) { this.position = position; }
    public void SetPositionFromTransform(Transform t) { this.position = t.position; }
    
    // Display function
    public void OnStartHoverGizmo() {
        if (this.gizmo_model == null) return;
        
        // TODO : tween
    }

    public void OnEndHoverGizmo() {}
}