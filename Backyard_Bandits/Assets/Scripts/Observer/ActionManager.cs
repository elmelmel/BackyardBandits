using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System;
using System.Collections.Generic;
using SaveLoad;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ActionManager : PersistentSingleton<ActionManager>
{

  public Scene mainScene;
  public float freezeTime = 3.0f; // The duration to freeze the game in seconds
  public Vector3 checkpoint;
  [SerializeField] List<GameObject> players;

  // Event handlers -----------------------------------------------------------------------------
  
  private System.Collections.IEnumerator FreezeCoroutine()
  {
    Time.timeScale = 0f; // Set the time scale to freeze the game

    yield return new WaitForSecondsRealtime(freezeTime); // Wait for the specified freeze time

    Time.timeScale = 1f; // Set the time scale back to normal
    SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
  }

  public virtual void OnSave(Save e)
  {
 
    SaveGameManager.CurrentSaveData.checkpointData.lastCheckpoint = checkpoint;
    //SaveGameManager.CurrentSaveData.checkpointData.players = players;
    SaveGameManager.SaveGame();
    
  }

  public virtual void OnLoad(Load e)
  {
    SaveGameManager.LoadGame();
    checkpoint = SaveGameManager.CurrentSaveData.checkpointData.lastCheckpoint;
    //players = SaveGameManager.CurrentSaveData.checkpointData.players;
  }
  public virtual void OnGameOver(GameOver e)
  {
    GameEventManager.Instance.Raise(new Save()); //save checkpoints before changing the scene
    StartCoroutine(FreezeCoroutine());
  }

  public virtual void OnReloadScene(ReloadScene e)
  {
    SceneManager.LoadScene("Prototype_06_13", LoadSceneMode.Single);
    GameEventManager.Instance.Raise(new Load());
  }
  public virtual void OnRespawn(Respawn e)
  {
    if(checkpoint != Vector3.zero) e._player.transform.position = checkpoint;
  }
  
  public virtual void OnCheckpoint(CheckpointEvent e)
  {
    checkpoint = e._checkpoint;
    e._checkpointObj.SetActive(false);
  }

  public virtual void OnRotateLight(RotateLight e)
  {
    Debug.Log("rotating object");
    StarterAssetsInputs _input = e.player.GetComponent<StarterAssetsInputs>();
    float _speed = e.player.GetComponent<ThirdPersonController>().RotationSpeed;
    float rotationAmount = _speed * Time.deltaTime;

    
    if (_input.rotateRight)
    {
      Quaternion newRotation = e.light.transform.rotation * Quaternion.Euler(0f, rotationAmount, 0f);
      if (Quaternion.Angle(newRotation, e.rota) <= e.max && Quaternion.Angle(newRotation, e.rota) >= e.min)
      {
        e.light.transform.rotation = newRotation;
      }
    }

    if (_input.rotateLeft)
    {
      Quaternion newRotation = e.light.transform.rotation * Quaternion.Euler(0f, -rotationAmount, 0f);
      if (Quaternion.Angle(newRotation, e.rota) >= e.min && Quaternion.Angle(newRotation, e.rota) <= e.max)
      {
        e.light.transform.rotation = newRotation;
      }
    }
  }


  public virtual void OnRotateLightX(RotateLightX e)
  {
    Debug.Log("rotating object");
    StarterAssetsInputs _input = e.player.GetComponent<StarterAssetsInputs>();
    float _speed = e.player.GetComponent<ThirdPersonController>().RotationSpeed;
    float rotationAmount = _speed * Time.deltaTime;

    if (_input.rotateRight)
    {
      Quaternion newRotation = e.light.transform.rotation * Quaternion.Euler(rotationAmount, 0f, 0f);
      if (Quaternion.Angle(newRotation, e.rota) <= e.max && Quaternion.Angle(newRotation, e.rota) >= e.min)
      {
        e.light.transform.rotation = newRotation;
      }
    }

    if (_input.rotateLeft)
    {
      Quaternion newRotation = e.light.transform.rotation * Quaternion.Euler(-rotationAmount, 0f, 0f);
      if (Quaternion.Angle(newRotation, e.rota) >= e.min && Quaternion.Angle(newRotation, e.rota) <= e.max)
      {
        e.light.transform.rotation = newRotation;
      }
    }
  }
  
  /// <summary>
  /// OnEnable, we start listening to events.
  /// </summary>
  protected virtual void OnEnable()
  {
    GameEventManager.Instance.AddListener<GameOver> (OnGameOver);
    GameEventManager.Instance.AddListener<Respawn> (OnRespawn);
    GameEventManager.Instance.AddListener<RotateLight> (OnRotateLight);
    GameEventManager.Instance.AddListener<RotateLightX> (OnRotateLightX);
    GameEventManager.Instance.AddListener<CheckpointEvent> (OnCheckpoint);
    GameEventManager.Instance.AddListener<Save> (OnSave);
    GameEventManager.Instance.AddListener<Load> (OnLoad);
    GameEventManager.Instance.AddListener<ReloadScene> (OnReloadScene);
  }

  /// <summary>
  /// OnDisable, we stop listening to events.
  /// </summary>
  protected virtual void OnDisable()
  {
    GameEventManager.Instance.RemoveListener<GameOver>(OnGameOver);
    GameEventManager.Instance.RemoveListener<Respawn>(OnRespawn);
    GameEventManager.Instance.RemoveListener<RotateLight>(OnRotateLight);
    GameEventManager.Instance.RemoveListener<RotateLightX>(OnRotateLightX);
    GameEventManager.Instance.RemoveListener<CheckpointEvent>(OnCheckpoint);
    GameEventManager.Instance.RemoveListener<Save>(OnSave);
    GameEventManager.Instance.RemoveListener<Load>(OnLoad);
    GameEventManager.Instance.RemoveListener<ReloadScene>(OnReloadScene);
  }
  
}

