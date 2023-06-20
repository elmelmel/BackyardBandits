using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ActionManager : PersistentSingleton<ActionManager>
{

  public Scene mainScene;

  // Event handlers -----------------------------------------------------------------------------


  public virtual void OnGameOver(GameOver e)
  {

    SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
  }

  public virtual void OnRespawn(Respawn e)
  {
    SceneManager.LoadScene("Prototype_06_13", LoadSceneMode.Single);
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
  }
  
}

