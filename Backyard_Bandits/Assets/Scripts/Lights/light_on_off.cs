using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light_on_off : MonoBehaviour
{
    public float onDuration = 2f;    // Duration in seconds for the light to stay on
    public float offDuration = 1f;   // Duration in seconds for the light to stay off

    private float timer;             // Timer to track the time
    private bool isLightOn;          // Flag to indicate if the light is currently on

    private Light lightComponent;    // Reference to the light component

    private void Start()
    {
        // Get the light component attached to the game object
        lightComponent = GetComponent<Light>();

        // Initialize the timer and light state
        timer = 0f;
        isLightOn = false;

        // Start with the light turned off
        lightComponent.enabled = false;
    }

    private void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to toggle the light
        if (isLightOn && timer >= onDuration)
        {
            // Turn off the light
            lightComponent.enabled = false;
            isLightOn = false;

            // Reset the timer
            timer = 0f;
        }
        else if (!isLightOn && timer >= offDuration)
        {
            // Turn on the light
            lightComponent.enabled = true;
            isLightOn = true;

            // Reset the timer
            timer = 0f;
        }
    }
}
