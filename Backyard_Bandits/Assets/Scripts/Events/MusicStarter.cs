using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    public enum ScenesMusic
    {
        Credits,
        TitleScreen,
        Prototype_06_13
    }

    public ScenesMusic sceneMusic;
    
    void Start()
    {
        if(sceneMusic == ScenesMusic.Credits)
            GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.CreditsSound));
        else if(sceneMusic == ScenesMusic.Prototype_06_13)
            GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.LevelStart));
        //else if(sceneMusic == ScenesMusic.TitleScreen)
            //GameEventManager.Instance.Raise(new SimpleEvent(SimpleEventType.TitleScreenSound));       //in case we get title screen music
    }
}
