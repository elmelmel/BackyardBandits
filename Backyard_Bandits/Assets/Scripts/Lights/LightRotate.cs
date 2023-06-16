using UnityEngine;

public class LightRotate : MonoBehaviour
{
    public float rotationSpeed = 30f;  // Rotation speed in degrees per second
    public float minAngle = 0f;       // Minimum angle of rotation
    public float maxAngle = 120f;     // Maximum angle of rotation

    private float currentAngle;       // Current angle of rotation
    private bool rotateForward;       // Flag to indicate the rotation direction

    private void Start()
    {
        // Initialize the current angle to the minimum angle
        currentAngle = minAngle;
        rotateForward = true;
    }

    private void Update()
    {
        // Calculate the desired rotation based on the speed and time
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Determine the rotation direction based on the flag
        float direction = rotateForward ? 1f : -1f;

        // Rotate the object around its own y-axis
        transform.Rotate(Vector3.up, rotationAmount * direction);

        // Update the current angle
        currentAngle += rotationAmount * direction;

        // Check if the object reaches the minimum or maximum angle
        if ((rotateForward && currentAngle >= maxAngle) || (!rotateForward && currentAngle <= minAngle))
        {
            // Reverse the rotation direction
            rotateForward = !rotateForward;

            // Special case for minimum angle of 0
            if (minAngle == 0f && currentAngle < 0f)
            {
                currentAngle = 0f;
            }
        }
    }
}