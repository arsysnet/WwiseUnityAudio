using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Stores informations about a particular sound event.
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEvent
 */
[System.Serializable]
public class SoundEvent
{
    /**
     * Stores the action type of the sound event
     */
    [SerializeField]
    public enum EEventAction
    {
        None = 0, //< Default action
        Play = 1, //< Will play a sound or a music
        Stop = 2, //< Will stop a sound or a music
        Rtpc = 3  //< Will call the RTPC method
    }

    /**
     * Stores the type of the sound event
     */
    [SerializeField]
    public enum EEventType
    {
        Sfx   = 0, //< Sfx   sound event
        Music = 1  //< Music sound event
    }

    /**
     * Sound events identifiers
     */
    [SerializeField] public int    EventID;
    [SerializeField] public string EventName;
    [SerializeField] public string EventDescription;

    /**
     * Stores all event targets (sounds)
     */
    [SerializeField]
    public List<AudioClip> EventTargets = new List<AudioClip>();

    /**
     * Sound events properties
     */
    [SerializeField] public EEventAction EventAction;
    [SerializeField] public EEventType   EventType;
    [SerializeField] public bool         EventIsRandom;
    [SerializeField] public bool         EventIsLooping;
    [SerializeField] public bool         EventIsVolumeRandom;
    [SerializeField] public bool         EventIsPitchRandom;
    [SerializeField] public int          EventMaxInstance;
    [SerializeField] public float        EventVolume;
    [SerializeField] public float        EventPitch;
    [SerializeField] public string       EventToStop;
    [SerializeField] public Vector2      EventPitchRandomRange;
    [SerializeField] public Vector2      EventVolumeRandomRange;
    [SerializeField] public AbstractRTPC EventRTPC;

    /**
     * Default constructor
     */
    public SoundEvent()
    {
        EventName        = "";
        EventDescription = "";

        // Hash the name to optimize names lookup
        EventID = EventName.GetHashCode();

        EventAction = EEventAction.None;
        EventType   = EEventType.Sfx;

        // Add a null element
        EventTargets.Add(null);

        EventIsRandom       = false;
        EventIsLooping      = false;
        EventIsVolumeRandom = false;
        EventIsPitchRandom  = false;

        EventMaxInstance = 1;
        EventVolume      = 0.5f;
        EventPitch       = 1.0f;
        EventToStop      = "";

        EventPitchRandomRange  = Vector2.zero;
        EventVolumeRandomRange = Vector2.zero;
        EventRTPC              = null;
    }

    /**
     * Copy constructor
     */
    public SoundEvent(SoundEvent other)
    {
        EventID          = other.EventID;
        EventName        = other.EventName;
        EventDescription = other.EventDescription;

        EventAction  = other.EventAction;
        EventType    = other.EventType;
        EventTargets = other.EventTargets;

        EventIsRandom       = other.EventIsRandom;
        EventIsLooping      = other.EventIsLooping;
        EventIsVolumeRandom = other.EventIsVolumeRandom;
        EventIsPitchRandom  = other.EventIsPitchRandom;

        EventMaxInstance = other.EventMaxInstance;
        EventVolume      = other.EventVolume;
        EventPitch       = other.EventPitch;
        EventToStop      = other.EventToStop;

        EventPitchRandomRange  = other.EventPitchRandomRange;
        EventVolumeRandomRange = other.EventVolumeRandomRange;
        EventRTPC              = other.EventRTPC;
    }
}