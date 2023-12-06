using UnityEngine;

public abstract class Device : PlacedPart {
    [Header("Device settings")]
    public KeyCode keycode;
    public float amplitude = 1.0f;

    protected Rigidbody rb;

    public override string BuildProprietesString() {
        string kc_string = keycode.ToString();
        string props = $"amp={amplitude},kc={kc_string}";

        if (this.GetPartObject().can_rotate) {
            props += ",rotation=" + this.GetParentAnchor().GetRotation();
        }

        return props;
    }

    // ENGINE FUNCTIONS
    private void Update() { if (Input.GetKey(keycode)) HandleActivation(); }

    // ABSTRACT FUNCTIONS
    public abstract void HandleActivation();

    // RIGIDBODY HANDLER
    public void GetRigidbodyFromCore() {
        PlacedPart parent = GetParentPart();
        while(parent != null) { parent = parent.GetParentPart(); }

        rb = parent.GetComponent<Rigidbody>();
    }

    // GETTERS AND SETTERS
    public void         SetRigibody(Rigidbody rb_ref)   { rb = rb_ref; }
    public Rigidbody    GetRigidbody()                  { return rb; }

    public float        GetAmplitude()                  { return this.amplitude; }
    public void         SetAmplitude(float a)           { this.amplitude = a; }
}