using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waiter());

    }

    IEnumerator waiter()
    {
        yield return new WaitForSecondsRealtime(3);
        GameEventManager.Instance.Raise(new Respawn());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
