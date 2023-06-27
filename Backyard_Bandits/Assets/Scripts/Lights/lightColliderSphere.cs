using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lightColliderSphere : MonoBehaviour
{
    private bool isTriggered = false;
    public float delay = 0.2f;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        // Check if the trigger is still true after a delay
        yield return new WaitForSeconds(delay);
        
        if (other.gameObject.CompareTag("Player") && isTriggered)
        {
            Debug.Log("light detection triggered");
            GameEventManager.Instance.Raise(new GameOver(other.gameObject));
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Set isTriggered to true when the trigger is entered
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset isTriggered to false when the trigger is exited
        isTriggered = false;
    }
    
}
