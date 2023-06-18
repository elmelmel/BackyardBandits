using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeCollider : MonoBehaviour
{
    public GameObject lightCollider;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightCollider.SetActive(false);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightCollider.SetActive(true);
        }
    }
}
