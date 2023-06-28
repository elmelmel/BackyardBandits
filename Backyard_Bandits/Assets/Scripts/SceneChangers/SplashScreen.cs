using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public Image splashImage; // The UI image to fade in
    public string nextScene; // The name of the next scene to load

    public float fadeInTime = 1.0f; // The time it takes for the image to fade in
    public float waitTime = 2.0f; // The time to wait before changing the scene

    private void Start()
    {
        splashImage.canvasRenderer.SetAlpha(0.0f); // Set the initial alpha of the image to 0

        // Call the FadeInImage method after a short delay
        Invoke("FadeInImage", 0.5f);
    }

    private void FadeInImage()
    {
        splashImage.CrossFadeAlpha(1.0f, fadeInTime, false); // Fade in the image over the specified time

        // Call the ChangeScene method after the specified wait time
        Invoke("ChangeScene", waitTime);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(nextScene); // Load the next scene
    }
    
}
