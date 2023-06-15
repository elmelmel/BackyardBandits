using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System;

[Serializable]
public class SoundSettings
{
  public bool MusicOn = true;
  public bool SfxOn = true;
}

public class SoundManager : PersistentSingleton<SoundManager>
{	
  [Header("Settings")]
  public SoundSettings Settings;

  [Header("Sound Effects")]
  /// true if the sound fx are enabled
  //public bool SfxOn=true;
  /// the sound fx volume
  [Range(0,1)]
  public float SfxVolume=1f;

  protected const string _saveFolderName = "Engine/";
  protected const string _saveFileName = "sound.settings";

  
  /// <summary>
  /// Plays a sound
  /// </summary>
  /// <returns>An audiosource</returns>
  /// <param name="sfx">The sound clip you want to play.</param>
  /// <param name="location">The location of the sound.</param>
  /// <param name="loop">If set to true, the sound will loop.</param>
  public virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, bool loop=false)
  {
    if (!Settings.SfxOn)
      return null;
    // we create a temporary game object to host our audio source
    GameObject temporaryAudioHost = new GameObject("TempAudio");
    // we set the temp audio's position
    temporaryAudioHost.transform.position = location;
    // we add an audio source to that host
    AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource; 
    // we set that audio source clip to the one in paramaters
    audioSource.clip = sfx; 
    // we set the audio source volume to the one in parameters
    audioSource.volume = SfxVolume;
    // we set our loop setting
    audioSource.loop = loop;
    // we start playing the sound
    audioSource.Play(); 

    if (!loop)
    {
      // we destroy the host after the clip has played
      Destroy(temporaryAudioHost, sfx.length);
    }

    // we return the audiosource reference
    return audioSource;
  }

  /// <summary>
  /// Stops the looping sounds if there are any
  /// </summary>
  /// <param name="source">Source.</param>
  public virtual void StopLoopingSound(AudioSource source)
  {
    if (source != null)
    {
      Destroy(source.gameObject);
    }
  }

  
  // Event handlers -----------------------------------------------------------------------------

  public virtual void OnPlayerReachGoal(PlayerReachGoalEvent e) {
    if (!e.isValid()) return;
    Debug.Log ("Reach Goal Direction"+e.direction);
    Debug.Log ("Reach Goal Target"+e.target);
    playerReachGoalSound(e);
  }


  public virtual void OnSimpleEvent(SimpleEvent e)
  {
    switch (e.eventType) {
      case (SimpleEventType.LevelStart): 
        Debug.Log ("LevelStart");
        levelStartSound();
        break;
      case (SimpleEventType.LevelComplete): 
        Debug.Log ("LevelComplete");
        levelCompleteSound();
        break;
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
  
  //	Sound feedback methods -----------------------------------------------------------------------------

  public AudioClip soundLevelStart;
  public AudioClip soundLevelComplete;
  public AudioClip soundPlayerReachGoalFront;
  public AudioClip soundPlayerReachGoalBehind;
  
  
  void levelStartSound()
  {
    Debug.Log ("LevelStart");
    PlaySound (soundLevelStart, this.transform.position);
  }

  void levelCompleteSound()
  {
    Debug.Log ("LevelComplete");
    PlaySound (soundLevelComplete, this.transform.position);
  }
  

  //	this method is an example how event data can be used to specify the feedback

  void playerReachGoalSound(PlayerReachGoalEvent e)
  {
    Debug.Log ("Reach Goal Sound ...");
    if( Vector3.Dot(e.target.forward, e.direction.normalized) < 0 )
    {
      Debug.Log("...hit target " + e.target.name + " from front");
      PlaySound (soundPlayerReachGoalFront, this.transform.position);
    }
    else
    {
      Debug.Log("...hit target " + e.target.name + " from behind");
      PlaySound (soundPlayerReachGoalBehind, this.transform.position);
    }
  }

}
