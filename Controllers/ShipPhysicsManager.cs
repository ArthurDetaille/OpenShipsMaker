using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPhysicsManager : MonoBehaviour
{
    // VARIABLES
    private Rigidbody rb;

    private void Awake() {
        PlacedPart core = this.GetComponent<PlacedPart>(); 
        if (core != null) InitializePhysics(core);
    }

    public void InitializePhysics(PlacedPart to_init) {
        rb = this.gameObject.AddComponent<Rigidbody>();

        rb.drag = 0;
        rb.angularDrag = 0;
        rb.useGravity = false;
        UpdateMass();
    }

    public static float ComputeMass(PlacedPart part) {
        float total = part.GetPartObject().mass;

        foreach(Anchor anchor in part.GetAnchors()) {
            PlacedPart child = anchor.GetChildPart();
            
            if (child != null) total += ComputeMass(child);
        }

        return total;
    }

    // GETTERS AND SETTERS
    public Rigidbody    GetRigidbody() { return this.rb; }
    public void         UpdateMass() { this.rb.mass = ComputeMass(this.GetComponent<PlacedPart>()); }
}
