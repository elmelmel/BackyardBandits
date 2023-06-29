using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    
    public float waitTime = 8.0f;

    private void Start()
    {
        Invoke("ChangeScene", waitTime);
    }
    

    private void ChangeScene()
    {
        SceneManager.LoadScene("Tutorial2");
    }
}
