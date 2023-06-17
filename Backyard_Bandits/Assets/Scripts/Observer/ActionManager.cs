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
    SceneManager.LoadScene(mainScene.name, LoadSceneMode.Single);
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
    
    
    /*
    Transform targetTransform = e.light.transform;

    Quaternion targetRotation = targetTransform.rotation; // Keep the current rotation as the initial target

    if (_input.rotateRight)
    {
      // Rotate the object to the right
      targetRotation *= Quaternion.Euler(0f, rotationAmount, 0f);
    }
    else if (_input.rotateLeft)
    {
      // Rotate the object to the left
      targetRotation *= Quaternion.Euler(0f, -rotationAmount, 0f);
    }

    // Clamp the rotation within the specified range
    float clampedRotationY = Mathf.Clamp(targetRotation.eulerAngles.y, e.min, e.max);
    targetRotation = Quaternion.Euler(0f, clampedRotationY, 0f);

    // Smoothly interpolate the rotation using Quaternion.Slerp
    targetTransform.rotation = Quaternion.Slerp(targetTransform.rotation, targetRotation, _speed * Time.deltaTime);
    */
    
  }


  /// <summary>
  /// OnEnable, we start listening to events.
  /// </summary>
  protected virtual void OnEnable()
  {
    GameEventManager.Instance.AddListener<GameOver> (OnGameOver);
    GameEventManager.Instance.AddListener<Respawn> (OnRespawn);
    GameEventManager.Instance.AddListener<RotateLight> (OnRotateLight);
  }

  /// <summary>
  /// OnDisable, we stop listening to events.
  /// </summary>
  protected virtual void OnDisable()
  {
    GameEventManager.Instance.RemoveListener<GameOver>(OnGameOver);
    GameEventManager.Instance.RemoveListener<Respawn>(OnRespawn);
    GameEventManager.Instance.RemoveListener<RotateLight>(OnRotateLight);
  }
  
}

