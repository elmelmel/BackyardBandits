using UnityEngine;

namespace GameEvents
{  

  //---------------------------------------------------------------------------------------------------
  // sample events without additional arguments

  public class GameEvent_Click : GameEvent {}
  public class GameEvent_CancelAction : GameEvent {}
  public class GameEvent_FinishLevel : GameEvent {}

  //---------------------------------------------------------------------------------------------------

  // sample events with additional arguments. Make sure these implement Validate()

  /// <summary>
  /// A list of the possible simplified Game Engine base events
  /// </summary>
  public enum SimpleEventType
  {
    LevelStart,
    LevelComplete,
    LevelEnd,
    Pause,
    UnPause,
    PlayerDeath,
    Respawn,
    StarPicked
  }
  
  public class SimpleEvent : GameEvent
  {
    public readonly SimpleEventType eventType;
    public SimpleEvent(SimpleEventType t)
    {
      eventType = t;
    }
    
    public override bool isValid() {
      return (eventType != null);
    }
  }
  
  public class PlayerReachGoalEvent : GameEvent
  {
    public readonly Transform target;
    public readonly Vector3 direction;

    public PlayerReachGoalEvent(Transform t, Vector3 d) {
      target = t;
      direction = d;
    }

    public override bool isValid ()
    {
      return target != null && direction.magnitude > 0;
    }
  }
  public class GameOver : GameEvent
  {

  }
  public class Respawn : GameEvent
  {

  }

  public class RotateLight : GameEvent
  {
    public readonly GameObject player;
    public readonly GameObject light;
    public readonly Quaternion rota;
    public readonly float min;
    public readonly float max;

    public RotateLight(GameObject p, GameObject l, Quaternion r, float mi, float ma)
    {
      player = p;
      light = l;
      rota = r;
      min = mi;
      max = ma;
    }
  }
  
}