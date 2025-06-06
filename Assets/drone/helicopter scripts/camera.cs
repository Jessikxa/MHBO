using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private Transform target;             // The current helicopter or object to follow

    public float distance = 5f;           // Distance behind the target
    public float height = 2f;             // Height above the target
    public float rotationSpeed = 5f;      // Mouse sensitivity

    public float minPitch = -30f;         // Min camera pitch angle
    public float maxPitch = 60f;          // Max camera pitch angle

    private float yaw = 0f;
    private float pitch = 15f;            // Start slightly downward

    void LateUpdate()
    {
        if (target == null) return;

        // Zoom control
        distance -= Input.GetAxis("Mouse ScrollWheel") * 5f;
        distance = Mathf.Clamp(distance, 2f, 20f);

        // Right mouse button to rotate
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // Orbit calculation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 finalPosition = target.position + Vector3.up * height + offset;

        transform.position = finalPosition;
        transform.LookAt(target.position + Vector3.up * height);
    }

    // Call this to update the target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // Reset orbit angles when switching
        yaw = newTarget.eulerAngles.y;
        pitch = 15f;
    }
}
