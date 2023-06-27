﻿using UnityEngine;
using UnityEngine.SceneManagement;

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
    GameOverSound,
    RespawnSound,
    Dialogue,
    JumpKit,
    JumpCub,
    LandKit,
    LandCub,
    FootstepsKit,
    FootstepsCub,
    LightRotate,
    FlowerpotPull,
    PipeClimb
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
    public readonly GameObject playerName;

    public GameOver(GameObject p)
    {
      playerName = p;
    }
  }
  public class Respawn : GameEvent
  {
    public readonly GameObject _player;

    public Respawn(GameObject p)
    {
      _player = p;
    }
  }
  public class CheckpointEvent : GameEvent
  {
    public readonly Vector3 _checkpoint;
    public readonly GameObject _checkpointObj;
    public CheckpointEvent(Vector3 c, GameObject co)
    {
      _checkpoint = c;
      _checkpointObj = co;
    }
  }

  public class Save : GameEvent
  {
    
  }

  public class Load : GameEvent
  {
    
  }

  public class ReloadScene : GameEvent
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
  public class RotateLightX : GameEvent
  {
    public readonly GameObject player;
    public readonly GameObject light;
    public readonly Quaternion rota;
    public readonly float min;
    public readonly float max;

    public RotateLightX(GameObject p, GameObject l, Quaternion r, float mi, float ma)
    {
      player = p;
      light = l;
      rota = r;
      min = mi;
      max = ma;
    }
  }

  public class NextScene : GameEvent
  {
    public readonly string nextScene;
    public readonly float waitingTime;

    public NextScene(string s, float w)
    {
      nextScene = s;
      waitingTime = w;
    }
  }
}