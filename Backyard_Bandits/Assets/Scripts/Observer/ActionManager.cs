using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System;
using UnityEngine.SceneManagement;

public class ActionManager : PersistentSingleton<ActionManager>
{

  public Scene mainScene;

  // Event handlers -----------------------------------------------------------------------------

  
  public virtual void OnGameOver(GameOver e) {
    
    SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
  }

  public virtual void OnRespawn(Respawn e)
  {
    SceneManager.LoadScene(mainScene.name, LoadSceneMode.Single);
  }
  /// <summary>
  /// OnEnable, we start listening to events.
  /// </summary>
  protected virtual void OnEnable()
  {
    GameEventManager.Instance.AddListener<GameOver> (OnGameOver);
    GameEventManager.Instance.AddListener<Respawn> (OnRespawn);
  }

  /// <summary>
  /// OnDisable, we stop listening to events.
  /// </summary>
  protected virtual void OnDisable()
  {
    GameEventManager.Instance.RemoveListener<GameOver>(OnGameOver);
    GameEventManager.Instance.RemoveListener<Respawn>(OnRespawn);
  }
  
}

