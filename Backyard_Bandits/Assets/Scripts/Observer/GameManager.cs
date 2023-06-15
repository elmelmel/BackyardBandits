using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System.Collections.Generic;

/// The game manager is a persistent singleton that handles global stuff, like e.g., scoring and time
public class GameManager : PersistentSingleton<GameManager>
{
  public override void Awake() {
    GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.LevelStart));
  }


  public virtual void OnSimpleEvent(SimpleEvent e) { }

  public virtual void OnPlayerReachGoal(PlayerReachGoalEvent e) {}

  
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
    GameEventManager.Instance.RemoveListener<SimpleEvent> (OnSimpleEvent);
    GameEventManager.Instance.RemoveListener<PlayerReachGoalEvent>(OnPlayerReachGoal);
  }

}
