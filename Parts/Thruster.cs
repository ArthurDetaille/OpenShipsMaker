using UnityEngine;

public class Thruster : Device {
    [Header("Thruster settings")]
    public float BasePower = 1.0f;

    public override void HandleActivation() {
        if (GetRigidbody() == null) return;

        Vector3 force = this.transform.forward * BasePower * GetAmplitude();
        rb.AddForceAtPosition(force, this.transform.position, ForceMode.Force);
    }
}