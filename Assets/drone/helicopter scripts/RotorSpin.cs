using UnityEngine;

public class RotorSpin : MonoBehaviour
{
    public bool isTailRotor = false;   // Checkbox to specify if this is a tail rotor
    public float maxSpeed = 1000f;     // Maximum speed of the rotor
    public float spinUpTime = 7f;      // Time it takes to spin up or down
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    private void Update()
    {
        // Press R to toggle rotor speed
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (targetSpeed == 0f)
            {
                targetSpeed = maxSpeed;  // Spin up the rotors
            }
            else
            {
                targetSpeed = 0f;  // Spin down the rotors
            }
        }

        // Lerp the rotor speed to the target speed over time
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (maxSpeed / spinUpTime) * Time.deltaTime);

        // Apply the rotation based on currentSpeed
        if (isTailRotor)
        {
            // Tail rotor rotates around the local Z-axis (or another axis based on your design)
            transform.Rotate(Vector3.right * currentSpeed * Time.deltaTime);
        }
        else
        {
            // Main rotor rotates around the local X-axis
            transform.Rotate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }
}
