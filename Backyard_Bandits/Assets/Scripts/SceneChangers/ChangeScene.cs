using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public enum AllScenes
    {
        h_daLogo,
        TitleScreen,
        Prototype_06_13,
        GameOverKit,
        GameOverCub,
        EndScene,
        Credits
    }

    public AllScenes nextScene;
    public float onScreenTime = 3.5f;
    public bool requireInput = false;
    public bool requireTrigger = false;
    
    private void OnEnable()
    {
        if (!requireInput && !requireTrigger)
        {
            Debug.Log("On enable on changeScnene called");
            GameEventManager.Instance.Raise(new NextScene(nextScene.ToString(), onScreenTime));
            Debug.Log("successful");
        }

    }
    void Update()
    {
        if(requireInput)
        {
            if (Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
            {
                GameEventManager.Instance.Raise(new NextScene(nextScene.ToString(), 0.0f));
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (requireTrigger && other.CompareTag("Player"))
            GameEventManager.Instance.Raise(new NextScene(nextScene.ToString(), 0.0f));
    }
}
