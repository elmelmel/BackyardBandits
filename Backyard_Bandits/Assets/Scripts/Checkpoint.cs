using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameEventManager.Instance.Raise(new CheckpointEvent(transform.position, gameObject));
        }
    }
}
