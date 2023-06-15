using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System;
using System.Collections.Generic;


public class ActionManager : PersistentSingleton<ActionManager>
{
  [Header("Observers")] 
  public List<GameObject> players;
  


  
  // Event handlers -----------------------------------------------------------------------------

  public virtual void OnPlayerReachGoal(PlayerReachGoalEvent e) {
    if (!e.isValid()) return;
    Debug.Log ("Reach Goal Direction"+e.direction);
    Debug.Log ("Reach Goal Target"+e.target);
  }


  public virtual void OnSimpleEvent(SimpleEvent e)
  {
    switch (e.eventType) {
      case (SimpleEventType.LevelEnd): 
        Debug.Log ("LevelEnd");
        break;
      case (SimpleEventType.Pause): 
        Debug.Log ("Pause");
        break;
      case (SimpleEventType.UnPause): 
        Debug.Log ("UnPause");
        break;
      case (SimpleEventType.PlayerDeath): 
        Debug.Log ("PlayerDeath");
        break;
      case (SimpleEventType.Respawn): 
        Debug.Log ("Respawn");
        break;
      case (SimpleEventType.StarPicked): 
        Debug.Log ("StarPicked");
        break;
    }
  } 

  /// <summary>
  /// OnEnable, we start listening to events.
  /// </summary>
  protected virtual void OnEnable()
  {
    GameEventManager.Instance.AddListener<SimpleEvent> (OnSimpleEvent);
    GameEventManager.Instance.AddListener<PlayerReachGoalEvent>(OnPlayerReachGoal);
  }

  /// <summary>
  /// OnDisable, we stop listening to events.
  /// </summary>
  protected virtual void OnDisable()
  {
      GameEventManager.Instance.RemoveListener<SimpleEvent>(OnSimpleEvent);
      GameEventManager.Instance.RemoveListener<PlayerReachGoalEvent>(OnPlayerReachGoal);
  }
  

}
