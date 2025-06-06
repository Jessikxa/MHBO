using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HelicopterController : MonoBehaviour
{
    public float yawSpeed = 50f;
    public float pitchSpeed = 50f;
    public float rollSpeed = 50f;
    public float liftForce = 10f;
    public float hoverForce = 9.81f;

    public float linearDrag = 0.5f;
    public float angularDrag = 2f;

    public float stabilizerYawForce = 10f;
    public float maxYawSpeed = 50f;

    public float yawDampingAtMaxSpeed = 0.5f;

    private Rigidbody rb;
    private float maxSpeed = 55f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000f;
        rb.linearDamping = linearDrag;
        rb.angularDamping = angularDrag;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        float yaw = 0f, pitch = 0f, roll = 0f, lift = 0f;

        if (Input.GetKey(KeyCode.E)) yaw = 1f;
        if (Input.GetKey(KeyCode.Q)) yaw = -1f;
        if (Input.GetKey(KeyCode.W)) pitch = 1f;
        if (Input.GetKey(KeyCode.S)) pitch = -1f;
        if (Input.GetKey(KeyCode.A)) roll = 1f;
        if (Input.GetKey(KeyCode.D)) roll = -1f;
        if (Input.GetKey(KeyCode.Space)) lift = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) lift = -1f;

        float currentSpeed = rb.linearVelocity.magnitude;
        float yawSpeedFactor = Mathf.Lerp(1f, yawDampingAtMaxSpeed, currentSpeed / maxSpeed);

        rb.AddTorque(transform.up * yaw * yawSpeed * yawSpeedFactor * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(transform.right * pitch * pitchSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(transform.forward * roll * rollSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        ApplyYawForceFromStabilizer();

        rb.AddForce(transform.up * (hoverForce + lift * liftForce), ForceMode.Acceleration);
    }

    void ApplyYawForceFromStabilizer()
    {
        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f;

        float horizontalSpeed = horizontalVelocity.magnitude;

        float speedThreshold = 2.78f; // 10 km/h
        float blendRange = 5f;        // Smooth transition over 5 m/s
        float blendFactor = Mathf.Clamp01((horizontalSpeed - speedThreshold) / blendRange);

        if (blendFactor <= 0f) return;

        Vector3 flightDir = horizontalVelocity.normalized;
        float currentYaw = transform.eulerAngles.y;
        float desiredYaw = Mathf.Atan2(flightDir.x, flightDir.z) * Mathf.Rad2Deg;
        float yawDiff = Mathf.DeltaAngle(currentYaw, desiredYaw);

        float yawCorrection = Mathf.Clamp(yawDiff, -maxYawSpeed, maxYawSpeed);
        float stabilizerStrength = stabilizerYawForce * horizontalSpeed * blendFactor;

        rb.AddTorque(transform.up * yawCorrection * stabilizerStrength * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
