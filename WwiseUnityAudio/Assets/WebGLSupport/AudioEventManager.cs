using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

/**
 * This monobehavior updates and manages all sound events 
 * and depending the settings enables or disables Wwise
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  AudioEventManager
 */
public class AudioEventManager : MonoBehaviour
{
    /**
     * @see AudioEventManagerInspector.cs
     */
    [HideInInspector]
    public bool UnityAudioState;

    [HideInInspector]
    public bool Verbose;

    /**
     * Stores all events at runtime
     */
    public List<SoundEvent> Events;

    /**
     * Stores all event targets at runtime
     */
    public List<GameObject> Targets;

    /**
     * Audio event manager settings
     */
    private        AudioMixerGroup   SFXMixer;
    private        AudioMixerGroup   MusicMixer;
    private        GameObject        SoundObject;
    private static AudioEventManager Instance;
    
    /**
     * Called when the object is loaded
     */
    void Awake()
    {
        if (UnityAudioState)
        {
            DisplayLogMessage("AudioEventManager : Unity Audio Initialization");
            InitializeUnityAudio();
            DisplayLogMessage("AudioEventManager : Unity Audio Initialized");
        }
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    /**
     * Monobehavior update
     */
    private void Update()
    {
        RemoveEvents(
            (target, hashCode) =>
            {
                if (!Application.isFocused || !Application.isPlaying)
                {
                    return false;
                }

                bool bAllFinished = true;
                AudioSource[] sources = target.GetComponents<AudioSource>();

                foreach(AudioSource source in sources)
                {
                    if(source.isPlaying)
                    {
                        bAllFinished = false;
                        break;
                    }
                }

                return bAllFinished;
            });
    }

    /**
     * Triggers an event (Play, Stop)
     * @param EventID The name of the event to trigger
     * @param Target The target object to attach the event (null by default)
     */
    public static void PostEvent(string EventID, GameObject Target = null)
    {
        if(AudioEventManager.Instance != null)
        {
            AudioEventManager.Instance.PostEvent_Wrapper(EventID, Target);
        }
        else
        {
            Debug.LogError("AudioEventManager : Fatal error, AudioEventManager instance is null");
        }
    }

    /**
     * Configures an event at runtime from a value
     * @param RTPCID The name of the rtpc
     * @param Value  The target value
     * @param Target The target object to attach the event (null by default)
     */
    public static void SetRTPCValue(string RTPCID, float Value, GameObject Target = null)
    {
        if (AudioEventManager.Instance != null)
        {
            AudioEventManager.Instance.SetRTPCValue_Wrapper(RTPCID, Value, Target);
        }
        else
        {
            Debug.LogError("AudioEventManager : Fatal error, AudioEventManager instance is null");
        }
    }

    /**
     * Wrapper for the post event call, choses between Unity Audio or Wwise
     * @param EventID The name of the event to trigger
     * @param Target The target object to attach the event (null by default)
     */
    private void PostEvent_Wrapper(string EventID, GameObject Target)
    {
        if(UnityAudioState)
        {
            PostEvent_Implementation(EventID);
        }
        else
        {

#if !UNITY_WEBGL
            AkSoundEngine.PostEvent(EventID, Target);
#endif

        }
    }

    /**
     * Wrapper for the rtpc event call, choses between Unity Audio or Wwise
     * @param RTPCID The name of the rtpc
     * @param Value  The target value
     * @param Target The target object to attach the event (null by default)
     */
    private void SetRTPCValue_Wrapper(string RTPCID, float Value, GameObject Target)
    {
        if (UnityAudioState)
        {
            SetRTPCValue_Implementation(RTPCID, Value);
        }
        else
        {

#if !UNITY_WEBGL
            AkSoundEngine.SetRTPCValue(RTPCID, Value, Target);
#endif

        }
    }

    /**
     * General implementation for the post event 
     * @param EventID the name of the event
     */
    private void PostEvent_Implementation(string EventID)
    {
        SoundEvent soundEvent = GetSoundEventFromID(EventID.GetHashCode());

        if(soundEvent == null)
        {
            DisplayWarningMessage("AudioEventManager : The event " + EventID + " was not found");
            return;
        }

        if(soundEvent.EventAction == SoundEvent.EEventAction.Play)
        {
            PostEventPlay_Implementation(soundEvent);
        }
        else if(soundEvent.EventAction == SoundEvent.EEventAction.Stop)
        {
            PostEventStop_Implementation(soundEvent);
        }
    }

    /**
     * Implementation for events of type "Play"
     */
    private void PostEventPlay_Implementation(SoundEvent soundEvent)
    {
        // Pre-condition
        if (GetEventInstanceCount(soundEvent.EventID) >= soundEvent.EventMaxInstance)
        {
            DisplayWarningMessage("AudioEventManager : Too many instances to instantiate " + soundEvent.EventName);
            return;
        }

        // Pre-condition
        if (soundEvent.EventTargets.Count == 0)
        {
            DisplayWarningMessage("AudioEventManager : The event " + soundEvent.EventName + " has no audio targets.");
            return;
        }

        // Instianting the sound object
        // Setting up the object
        GameObject go = Instantiate(SoundObject, this.transform) as GameObject;
        go.name       = soundEvent.EventName;

        AudioSource      source = go.GetComponent<AudioSource>();
        SoundEventHandle handle = go.GetComponent<SoundEventHandle>();
        handle.EventID          = soundEvent.EventID;

        
        if (soundEvent.EventIsRandom)
        {
            // We should add only one audio target
            int targetIndex = Random.Range(0, soundEvent.EventTargets.Count);
            source.clip     = soundEvent.EventTargets[targetIndex];
        }
        else
        {
            source.clip = soundEvent.EventTargets[0];
        
            int sourcesToAdd = soundEvent.EventTargets.Count - 1;
            for(int nTarget = 0; nTarget < sourcesToAdd; ++nTarget)
            {
                AudioSource newSource = go.AddComponent<AudioSource>();
                newSource.clip = soundEvent.EventTargets[nTarget + 1];
            }
        }
        
        // General audio source settings
        AudioSource[] sources = go.GetComponents<AudioSource>();
        int sourcesCount = sources.Length;

        for(int nSource = 0; nSource < sourcesCount; ++nSource)
        {
            // Mixer setting
            AudioMixerGroup mixer = null;
            if(soundEvent.EventType == SoundEvent.EEventType.Music)
            {
                mixer = MusicMixer;
            }
            else
            {
                mixer = SFXMixer;
            }
            
            sources[nSource].outputAudioMixerGroup = mixer;

            float pitch  = soundEvent.EventPitch;
            float volume = soundEvent.EventVolume;

            // Volume and Pitch settings
            if (soundEvent.EventIsPitchRandom)
            {
                pitch += Random.Range(soundEvent.EventPitchRandomRange.x, soundEvent.EventPitchRandomRange.y);
            }
            
            if (soundEvent.EventIsVolumeRandom)
            {
                volume += Random.Range(soundEvent.EventVolumeRandomRange.x, soundEvent.EventVolumeRandomRange.y);
            }

            sources[nSource].pitch  = pitch;
            sources[nSource].volume = volume;

            if(soundEvent.EventIsLooping)
            {
                sources[nSource].loop = true;
            }

            // Plays all sources
            sources[nSource].Play();
        }
         
         // Buffers the go
         Targets.Add(go);
    }

    /**
     * Implementation for events of type "Stop"
     */
    private void PostEventStop_Implementation(SoundEvent soundEvent)
    {
        RemoveEvents(
            (target, hashCode) => 
            {
                SoundEventHandle handle = target.GetComponent<SoundEventHandle>();
                return handle.EventID == hashCode;
            }, 
            soundEvent.EventToStop.GetHashCode());
    }

    /**
     * General implementation for the rtpc event
     * @param RTPCID the name of the rtpc
     * @param Value the target value
     */
    private void SetRTPCValue_Implementation(string RTPCID, float Value)
    {
        SoundEvent rtpcEvent = GetSoundEventFromID(RTPCID.GetHashCode());

        // Precondition
        if (rtpcEvent == null)
        {
            DisplayWarningMessage("AudioEventManager : The rtpc event " + RTPCID + " was not found");
            return;
        }

        GameObject soundObject = GetTargetFromID(rtpcEvent.EventToStop.GetHashCode());

        if(soundObject == null)
        {
            DisplayLogMessage("AudioEventManager : No target found to apply the RTPC");
            return;
        }

        AudioSource[] sources = soundObject.GetComponents<AudioSource>();

        // Builds the parameters pack
        RTPCEventParameters parameters = new RTPCEventParameters();

        parameters.MusicMixer = MusicMixer;
        parameters.SfxMixer   = SFXMixer;
        parameters.AudioSources.AddRange(sources);

        rtpcEvent.EventRTPC.OnRTPCUpdate(parameters, Value);
    }

    /**
     * Loads all needed resources for the audio event manager
     * when using unity audio
     */
    private void InitializeUnityAudio()
    {
        // Adds the audio listener
        this.gameObject.AddComponent<AudioListener>();

        SoundEventDatabase databaseInstance = LoadGenericResource(
            "Databases/SoundEventDatabase",
            "Event database successfully loaded",
            "Unable to load the events database") as SoundEventDatabase;

        SoundObject = LoadGenericResource(
            "Prefabs/SoundObject",
            "Sound object successfully loaded",
            "Unable to load the sound object prefab") as GameObject;

        AudioMixer _musicMixer = LoadGenericResource(
            "Mixers/MusicMixer",
            "Music mixer successfully loaded",
            "Unable to load the music mixer") as AudioMixer;

        AudioMixer _sfxMixer = LoadGenericResource(
            "Mixers/SFXMixer",
            "SFX mixer successfully loaded",
            "Unable to load the sfx mixer") as AudioMixer;

        MusicMixer = _musicMixer.FindMatchingGroups("Master")[0];
        SFXMixer   = _sfxMixer.FindMatchingGroups  ("Master")[0];

        if (databaseInstance != null)
        {
            Events = databaseInstance.Events;
        }
    }

    /**
     * Helper methods to load resources from resources folders
     * @param path The path to the resource
     * @param successMessage The message to display in case of success
     * @param errorMessage The message to display in case of failure
     */
    private Object LoadGenericResource(string path, string successMessage, string errorMessage)
    {
        Object loaded = Resources.Load(path);

        if(loaded == null)
        {
            DisplayErrorMessage("AudioEventManager : " + errorMessage);
        }
        else
        {
            DisplayLogMessage("AudioEventManager : " + successMessage);
        }

        return loaded;
    }

    /**
     * Finds a sound event from an ID
     * @param soundEventID The id of the sound event to find
     * @return A reference on the event or null if not found
     */
    private SoundEvent GetSoundEventFromID(int soundEventID)
    {
        int eventCount = Events.Count;
        for(int nEvent = 0; nEvent < eventCount; ++nEvent)
        {
            if (Events[nEvent].EventID == soundEventID)
            {
                return Events[nEvent];
            }
        }

        return null;
    }

    /**
     * Finds an event target from an ID
     * @param soundEventID The id of the sound target to find
     * @return A reference on the target or null if not found
     */
    private GameObject GetTargetFromID(int soundEventID)
    {
        foreach (GameObject soundObject in Targets)
        {
            SoundEventHandle handle = soundObject.GetComponent<SoundEventHandle>();

            if (handle.EventID == soundEventID)
            {
                return soundObject;
            }
        }

        return null;
    }

    /**
     * Removes all events that matches the predicate
     * @param predicate The predicate to satisfy
     * @param hashCode The hashcode to remove specific events
     */
    private void RemoveEvents(System.Func<GameObject, int, bool> predicate, int hashCode = 0)
    {
        // Reverse loop deletion
        for (int nObject = Targets.Count - 1; nObject >= 0; --nObject)
        {
            // This event should be deleted
            if (predicate(Targets[nObject], hashCode))
            {
                // Destroys the gameobject
                // Stops all audio sources
                Destroy(Targets[nObject]);
                Targets.RemoveAt(nObject);
            }
        }
    }

    /**
     * Returns the number of instances of a specific events
     * @param hashCode
     * @return the number of instances, could be zero
     */
    private int GetEventInstanceCount(int hashCode)
    {
        int instanceCount = 0;
        foreach(GameObject soundObject in Targets)
        {
            SoundEventHandle handle = soundObject.GetComponent<SoundEventHandle>();

            if(handle.EventID == hashCode)
            {
                instanceCount += 1;
            }
        }

        return instanceCount;
    }

    /**
     * Display a log message
     */
    private void DisplayLogMessage(string message)
    {
        if (Verbose)
            Debug.Log(message);
    }

    /**
     * Display a warning message
     */
    private void DisplayWarningMessage(string message)
    {
        if (Verbose)
            Debug.LogWarning(message);
    }

    /**
     * Display an error message
     */
    private void DisplayErrorMessage(string message)
    {
        if (Verbose)
            Debug.LogError(message);
    }
}
