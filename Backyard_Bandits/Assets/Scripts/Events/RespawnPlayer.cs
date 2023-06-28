using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.Instance.Raise(new Respawn(gameObject));

    }
} 
