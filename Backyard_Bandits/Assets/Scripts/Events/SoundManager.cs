using UnityEngine;
using System.Collections;
using Tools;
using GameEvents;
using System;
using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

[Serializable]
public class SoundSettings
{
  public bool MusicOn = true;
  public bool SfxOn = true;
}

public class SoundManager : PersistentSingleton<SoundManager>
{
  [Header("Settings")] public SoundSettings Settings;

  [Header("Sound Effects")]
  /// true if the sound fx are enabled
  //public bool SfxOn=true;
  /// the sound fx volume
  [Range(0, 1)]
  public float MusicVolume = 1f;

  [Range(0, 1)] public float SfxVolume = 1f;

  
  [Range(2.5f, 3.5f)] public float diaMin;
  [Range(3.5f, 8.5f)] public float diaMax;
  
  protected const string _saveFolderName = "Engine/";
  protected const string _saveFileName = "sound.settings";
  private List<AudioSource> currentAudioSource = new List<AudioSource>(); // Track the currently playing sounds

/// <summary>
/// Plays a sound
/// </summary>
/// <returns>An AudioSource</returns>
/// <param name="sfx">The sound clips you want to play.</param>
/// <param name="location">The location of the sound.</param>
/// <param name="loop">If set to true, the sound will loop.</param>
public virtual AudioSource PlaySound(List<AudioClip> sfx, Vector3 location, bool loop = false)
{
    if (!Settings.SfxOn || sfx.Count == 0)
        return null;

    // Check if the same sound is already playing
    foreach (AudioSource audioSources in currentAudioSource)
    {
        if (audioSources != null && audioSources.clip != null && sfx.Contains(audioSources.clip) && audioSources.isPlaying)
        {
            // Sound is already playing, so no need to play it again
            return audioSources;
        }
    }

    int randomIndex = UnityEngine.Random.Range(0, sfx.Count); // Generate a random index within the range of the soundFootstepsKit array

    AudioClip soundClip = sfx[randomIndex];
    
    // Create a new temporary audio host GameObject
    GameObject temporaryAudioHost = new GameObject("TempAudio");
    DontDestroyOnLoad(temporaryAudioHost);
  

  
    // Add an AudioSource component to the temporary audio host
    AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>();

    // Set the audio source properties
    audioSource.clip = soundClip;
    audioSource.volume = SfxVolume;
    audioSource.loop = loop;

    // Play the sound
    audioSource.Play();

    if (!loop)
    {
      // Determine the clip length
      float clipLength = soundClip.length;

      // Coroutine to destroy the temporary audio host and remove the AudioSource after the clip finishes playing
      IEnumerator DestroyAudioHost(float duration)
      {
        yield return new WaitForSeconds(duration);
        Destroy(temporaryAudioHost);
        currentAudioSource.Remove(audioSource);
      }

      StartCoroutine(DestroyAudioHost(clipLength));
    }
  
    

    // Update the currently playing sound
    currentAudioSource.Add(audioSource);

    // Return the AudioSource reference of the first audio source in the list
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

        // Remove the stopped sound from the currentAudioSource list
        foreach (AudioSource audioSources in currentAudioSource)
        {
            if (audioSources == source)
            {
                currentAudioSource.Remove(source);
                break;
            }
        }
    }
}


// Event handlers -----------------------------------------------------------------------------
  public virtual void OnSimpleEvent(SimpleEvent e)
  {
    switch (e.eventType)
    {
      case (SimpleEventType.LevelStart):
        levelStartSound();
        break;
      case(SimpleEventType.CreditsSound):
        creditsSound();
        break;
      case (SimpleEventType.GameOverSound):
        gameOverSound();
        break;
      case (SimpleEventType.RespawnSound):
        respawnSound();
        break;
      case (SimpleEventType.Dialogue):
        DialogueSound();
        break;
      case (SimpleEventType.JumpKit):
        jumpKitSound();
        break;
      case (SimpleEventType.JumpCub):
        jumpCubSound();
        break;
      case (SimpleEventType.LandKit):
        landKitSound();
        break;
      case (SimpleEventType.LandCub):
        landCubSound();
        break;
      case (SimpleEventType.FootstepsKit):
        footstepsKitSound();
        break;
      case (SimpleEventType.FootstepsCub):
        footstepsCubSound();
        break;
      case (SimpleEventType.LightRotate):
        lightRotateSound();
        break;
      case (SimpleEventType.FlowerpotPull):
        flowerpotPullSound();
        break;
      case (SimpleEventType.PipeClimb):
        pipeClimbSound();
        break;
    }
  }

  /// <summary>
  /// OnEnable, we start listening to events.
  /// </summary>
  protected virtual void OnEnable()
  {
    GameEventManager.Instance.AddListener<SimpleEvent>(OnSimpleEvent);
    
  }

  /// <summary>
  /// OnDisable, we stop listening to events.
  /// </summary>
  protected virtual void OnDisable()
  {
    GameEventManager.Instance.RemoveListener<SimpleEvent>(OnSimpleEvent);
  }

  private System.Collections.IEnumerator DialogueWait()
  {
    float dialogueWaitingTime = UnityEngine.Random.Range(diaMin, diaMax);
    int diaOrder =  UnityEngine.Random.Range(1, 3);
    
    if (diaOrder % 2 == 0)
    {
      PlaySound(soundKitDialogueCall, this.transform.position);
      yield return new WaitForSecondsRealtime(dialogueWaitingTime);
      PlaySound(soundCubDialogueResponse, this.transform.position);
    }
    else
    {
      PlaySound(soundCubDialogueCall, this.transform.position);
      yield return new WaitForSecondsRealtime(dialogueWaitingTime);
      PlaySound(soundKitDialogueResponse, this.transform.position);
    }
  }
  
  //	Sound feedback methods -----------------------------------------------------------------------------

  public List<AudioClip> soundBackgroundMusic;
  public List<AudioClip> soundCreditsMusic;
  public List<AudioClip> soundGameOver;
  public List<AudioClip> soundRespawn;
  public List<AudioClip> soundKitDialogueCall;
  public List<AudioClip> soundCubDialogueCall;
  public List<AudioClip> soundKitDialogueResponse;
  public List<AudioClip> soundCubDialogueResponse;
  public List<AudioClip> soundJumpKit;
  public List<AudioClip> soundJumpCub;
  public List<AudioClip>soundLandKit;
  public List<AudioClip> soundLandCub;
  public List<AudioClip> soundFootstepsKit;
  public List<AudioClip> soundFootstepsCub;
  public List<AudioClip> soundLightRotate;
  public List<AudioClip> soundFlowerpotPull;
  public List<AudioClip> soundPipeClimb;
  
  void levelStartSound()
  {
    if (Settings.MusicOn)
    {
      // Set the audio source properties
      foreach (AudioClip bgm in soundBackgroundMusic)
      {
        GameObject backgroundMusicHost = new GameObject("backgroundMusicHost");
        AudioSource audioSource = backgroundMusicHost.AddComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.volume = MusicVolume;
        audioSource.loop = true;

        audioSource.Play();
      }
    } 
  }

  void creditsSound()
  {
    foreach (AudioClip bgm in soundCreditsMusic)
    {
      GameObject creditsMusicHost = new GameObject("creditsMusicHost");
      AudioSource audioSource = creditsMusicHost.AddComponent<AudioSource>();
      audioSource.clip = bgm;
      audioSource.volume = MusicVolume;
      audioSource.loop = true;

      audioSource.Play();
    }
  } 
  
  void gameOverSound()
  {
    PlaySound(soundGameOver, this.transform.position);
  }

  void respawnSound()
  {
    
  }

  void DialogueSound()
  {
    StartCoroutine(DialogueWait());
  }

  void jumpKitSound()
  {
    PlaySound(soundJumpKit, this.transform.position);
  }

  void jumpCubSound()
  {
    PlaySound(soundJumpCub, this.transform.position);
  }

  void landKitSound()
  {
    PlaySound(soundLandKit, this.transform.position);
  }

  void landCubSound()
  {
    PlaySound(soundLandCub, this.transform.position);
  }

  void footstepsKitSound()
  {
   
    PlaySound(soundFootstepsKit, this.transform.position);
      
  }

  void footstepsCubSound()
  {
    PlaySound(soundFootstepsCub, this.transform.position);
  }

  void lightRotateSound()
  {
    PlaySound(soundLightRotate, this.transform.position);
  }

  void flowerpotPullSound()
  {
    
    PlaySound(soundFlowerpotPull, this.transform.position);
      
  }

  void pipeClimbSound()
  {
    
    PlaySound(soundPipeClimb, this.transform.position);
      
  }
}
