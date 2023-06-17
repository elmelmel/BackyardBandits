using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class LightRotatePlayer : MonoBehaviour
{
    
    [SerializeField] private float minRotationAngle = 0f;
    [SerializeField] private float maxRotationAngle = 90f;
    private Quaternion _rotation;

    private void Start()
    {
        _rotation = transform.rotation;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameEventManager.Instance.Raise(new RotateLight(collision.gameObject, this.gameObject, _rotation, minRotationAngle, maxRotationAngle));
        }
    }
}
