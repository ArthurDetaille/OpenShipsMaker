using UnityEngine;

public abstract class Device : PlacedPart {
    public string keycode = null;
    public float amplitude = 1.0f;

    public override string BuildProprietesString() {
        string kc_string = keycode == null ? "none" : keycode;
        string props = $"amp={amplitude},kc={kc_string}";

        if (this.GetPartObject().can_rotate) {
            props += ",rotation=" + this.GetParentAnchor().GetRotation();
        }

        return props;
    }
}