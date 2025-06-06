using UnityEngine;

public class SimpleFly : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hides and locks the cursor
    }

    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right
        float moveY = 0;
        if (Input.GetKey(KeyCode.Space)) moveY = 1; // Space to go up
        if (Input.GetKey(KeyCode.LeftShift)) moveY = -1; // Shift to go down
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down

        Vector3 moveDirection = transform.TransformDirection(new Vector3(moveX, moveY, moveZ));
        transform.position += moveDirection * speed * Time.deltaTime;

        // Mouse Look (Rotation)
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Prevent flipping

        transform.localRotation = Quaternion.Euler(rotationX, transform.localRotation.eulerAngles.y + mouseX, 0);
    }
}
