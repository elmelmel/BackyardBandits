using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSphereOnOff : MonoBehaviour
{
    public float onDuration = 2f;    // Duration in seconds for the light to stay on
    public float offDuration = 1f;   // Duration in seconds for the light to stay off

    private float timer;             // Timer to track the time
    private bool isSphereOn;          // Flag to indicate if the light is currently on

    private CapsuleCollider colliderComponent;    // Reference to the light component

    private void Start()
    {
        
        // Initialize the timer and light state
        timer = 0f;
        isSphereOn = false;

        colliderComponent = GetComponent<CapsuleCollider>();
        
        // Start with the light turned off
        colliderComponent.enabled = false;
    }

    private void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to toggle the light
        if (isSphereOn && timer >= onDuration)
        {
            // Turn off the light
            colliderComponent.enabled = false;
            isSphereOn = false;

            // Reset the timer
            timer = 0f;
        }
        else if (!isSphereOn && timer >= offDuration)
        {
            // Turn on the light
            colliderComponent.enabled = true;
            isSphereOn = true;

            // Reset the timer
            timer = 0f;
        }
    }
}
