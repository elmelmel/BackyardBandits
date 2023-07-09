using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    public Transform cameraTransform;

    private void Update()
    {
        // Rotate the object to face the camera
        transform.rotation = cameraTransform.rotation;
    }
}
