using UnityEngine;

public class LightRotate : MonoBehaviour
{
    public float rotationSpeed = 30f;  // Rotation speed in units per second
    public float minY = 0f;           // Minimum y-axis value of rotation
    public float maxY = 120f;         // Maximum y-axis value of rotation

    private float currentY;           // Current y-axis value of rotation
    private bool rotateForward;       // Flag to indicate the rotation direction

    private void Start()
    {
        // Initialize the current y-axis value to the minimum value
        currentY = minY;
        rotateForward = true;
    }

    private void Update()
    {
        // Calculate the desired rotation based on the speed and time
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Determine the rotation direction based on the flag
        float direction = rotateForward ? 1f : -1f;

        // Rotate the object around its own y-axis
        transform.localRotation = Quaternion.Euler(transform.rotation.x, currentY, transform.rotation.z);

        // Update the current y-axis value
        currentY += rotationAmount * direction;

        // Check if the object reaches the minimum or maximum y-axis value
        if ((rotateForward && currentY >= maxY) || (!rotateForward && currentY <= minY))
        {
            // Reverse the rotation direction
            rotateForward = !rotateForward;
        }
    }
}