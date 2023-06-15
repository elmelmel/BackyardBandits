using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        SceneManager.LoadScene("Prototype", LoadSceneMode.Single);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
