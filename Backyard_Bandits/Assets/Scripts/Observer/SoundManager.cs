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
  [Range(0, 1)]
  public float MusicVolume = 1f;
  [Range(0, 1)]
  public float SfxVolume=1f;

  protected const string _saveFolderName = "Engine/";
  protected const string _saveFileName = "sound.settings";
  private AudioSource currentAudioSource; // Track the currently playing sound
  
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

    // Check if the same sound is already playing
    if (currentAudioSource != null && currentAudioSource.clip == sfx && currentAudioSource.isPlaying)
    {
      // Sound is already playing, so no need to play it again
      return currentAudioSource;
    }

    // Create a new temporary audio host GameObject
    GameObject temporaryAudioHost = new GameObject("TempAudio");
    DontDestroyOnLoad(temporaryAudioHost);

    // Add an AudioSource component to the temporary audio host
    AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>();

    // Set the audio source properties
    audioSource.clip = sfx;
    audioSource.volume = SfxVolume;
    audioSource.loop = loop;

    // Play the sound
    audioSource.Play();

    if (!loop)
    {
      // Destroy the temporary audio host after the clip has played
      Destroy(temporaryAudioHost, sfx.length);
    }

    // Update the currently playing sound
    currentAudioSource = audioSource;

    // Return the AudioSource reference
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
  public virtual void OnSimpleEvent(SimpleEvent e)
  {
    switch (e.eventType) {
      case (SimpleEventType.LevelStart): 
        levelStartSound();
        break;
      case (SimpleEventType.GameOverSound): 
        gameOverSound();
        break;
      case (SimpleEventType.RespawnSound):
        respawnSound();
        break;
      case(SimpleEventType.JumpKit):
        jumpKitSound();
        break;
      case(SimpleEventType.JumpCub):
        jumpCubSound();
        break;
      case(SimpleEventType.LandKit):
        landKitSound();
        break;
      case(SimpleEventType.LandCub):
        landCubSound();
        break;
      case(SimpleEventType.FootstepsKit):
        footstepsKitSound();
        break;
      case(SimpleEventType.FootstepsCub):
        footstepsCubSound();
        break;
      case(SimpleEventType.LightRotate):
        lightRotateSound();
        break;
      case(SimpleEventType.FlowerpotPull):
        flowerpotPullSound();
        break;
      case(SimpleEventType.PipeClimb):
        pipeClimbSound();
        break;
    }
  } 

  /// <summary>
  /// OnEnable, we start listening to events.
  /// </summary>
  protected virtual void OnEnable()
  {
    GameEventManager.Instance.AddListener<SimpleEvent> (OnSimpleEvent);

    if (Settings.MusicOn)
    {
      GameObject backgroundMusicHost = new GameObject("backgroundMusicHost");
      AudioSource audioSource = backgroundMusicHost.AddComponent<AudioSource>();

      // Set the audio source properties
      audioSource.clip = soundBackgroundMusic;
      audioSource.volume = MusicVolume;
      audioSource.loop = true;

      audioSource.Play();
    }

  }

  /// <summary>
  /// OnDisable, we stop listening to events.
  /// </summary>
  protected virtual void OnDisable()
  {
      GameEventManager.Instance.RemoveListener<SimpleEvent>(OnSimpleEvent);
  }
  
  //	Sound feedback methods -----------------------------------------------------------------------------

  public AudioClip soundBackgroundMusic;
  public AudioClip soundLevelStart;
  public AudioClip soundGameOver;
  public AudioClip soundRespawn;
  public AudioClip soundJumpKit;
  public AudioClip soundJumpCub;
  public AudioClip soundLandKit;
  public AudioClip soundLandCub;
  public AudioClip soundFootstepsKit;
  public AudioClip soundFootstepsCub;
  public AudioClip soundLightRotate;
  public AudioClip soundFlowerpotPull;
  public AudioClip soundPipeClimb;
  
  void levelStartSound()
  {
    PlaySound (soundLevelStart, this.transform.position);
  }

  void gameOverSound()
  {
    PlaySound (soundGameOver, this.transform.position);
  }
  void respawnSound()
  {
    PlaySound (soundRespawn, this.transform.position);
  }

  void jumpKitSound()
  {
    PlaySound (soundJumpKit, this.transform.position);
  }

  void jumpCubSound()
  {
    PlaySound (soundJumpCub, this.transform.position);
  }

  void landKitSound()
  {
    PlaySound (soundLandKit, this.transform.position);
  }
  void landCubSound()
  {
    PlaySound (soundLandCub, this.transform.position);
  }

  void footstepsKitSound()
  {
    PlaySound (soundFootstepsKit, this.transform.position);
  }

  void footstepsCubSound()
  {
    PlaySound (soundFootstepsCub, this.transform.position);
  }

  void lightRotateSound()
  {
    PlaySound (soundLightRotate, this.transform.position);
  }

  void flowerpotPullSound()
  {
    PlaySound (soundFlowerpotPull, this.transform.position);
  }

  void pipeClimbSound()
  {
    PlaySound (soundPipeClimb, this.transform.position);
  }

}
