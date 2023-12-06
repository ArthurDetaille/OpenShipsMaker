using UnityEngine;
using System.Collections.Generic;

public class Thruster : Device {
    [Header("Thruster settings")]
    [SerializeField] private float base_power = 1.0f;

    public override void HandleActivation() {
        if (GetRigidbody() == null) return;

        Vector3 force = this.transform.forward * base_power * GetAmplitude();
        rb.AddForceAtPosition(force, this.transform.position, ForceMode.Force);
    }

    public override string BuildProprietesString() {
        string kc_string = keycode.ToString();
        string props = $"amp={amplitude},kc={kc_string},base_power={base_power}";

        if (this.GetPartObject().can_rotate) {
            props += ",rotation=" + this.GetParentAnchor().GetRotation();
        }

        return props;
    }

    public override void SetFromProprietes(SaveAnchorResult result) {
        float bp = float.Parse(result.properties["base_power"]);
        float amp = float.Parse(result.properties["amp"]);

        SetBasePower(bp);
        SetAmplitude(amp);

        // TODO: Set keycode from string
    }

    // GETTERS AND SETTERS
    public float GetBasePower() { return this.base_power; }
    public void SetBasePower(float bp) { this.base_power = bp; }
}