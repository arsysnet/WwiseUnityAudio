#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Stores all sounds events.
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEventDatabase
 */
[System.Serializable]
[CreateAssetMenu(
    fileName = "SoundEventDatabase", 
    menuName = "WebGL/Sound Event Database")]
public class SoundEventDatabase : ScriptableObject
{
    /**
     * Stores all sound events
     */
    [SerializeField]
    public List<SoundEvent> Events = new List<SoundEvent>();

    /**
     * Avoid object reset when on scene changed, or play/stop events
     */
    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}