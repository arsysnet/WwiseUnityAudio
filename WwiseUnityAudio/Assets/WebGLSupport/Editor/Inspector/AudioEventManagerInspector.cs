#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;

/**
 * Custom inspector for the audio event manager
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  AudioEventManagerInspector
 */
[CustomEditor(typeof(AudioEventManager))]
public class AudioEventManagerInspector : Editor
{
    /**
     * Called to draw the custom inspector
     */
    public override void OnInspectorGUI()
    {
        AudioEventManager manager = (AudioEventManager)target;

#if UNITY_EDITOR

        // Gets the audio settings from the unity project settings folder
        Object             audioSettings      = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/AudioManager.asset")[0];
        SerializedObject   serializedSettings = new SerializedObject(audioSettings);
        SerializedProperty disableProperty    = serializedSettings.FindProperty("m_DisableAudio");

        bool bUpdated = false;
        if (manager.UnityAudioState)
        {
            if(disableProperty.boolValue)
            {
                bUpdated = true;
                disableProperty.boolValue = false;
            }
        }
        else
        {
            if (!disableProperty.boolValue)
            {
                bUpdated = true;
                disableProperty.boolValue = true;
            }
        }

        if(bUpdated)
        {
            serializedSettings.ApplyModifiedProperties();
        }
#endif

        EditorGUILayout.HelpBox(
            "If you check this checkbox, it will disable Wwise and enable " +
            "Unity audio from the project settings. It will also add an" +
            "Audio Listener to this game object. All calls to Wwise will " +
            "be nullified and replaced by a call to the audio event manager. " +
            "In the case of a RTPC event, your callbacks will be called. But " +
            "you still have to remove the Wwise folder (the one in the assets " +
            "folder) for the time of the build. Once you have built your game " +
            "you can put the wwise folder back into your assets folder.",
            MessageType.Warning);

        manager.Verbose = EditorGUILayout.Toggle(new GUIContent("Verbose"), manager.Verbose);

        if (!Application.isPlaying)
        {
            manager.UnityAudioState = EditorGUILayout.Toggle(new GUIContent("Use Unity Audio"), manager.UnityAudioState);
        }
        else
        {
            if (manager.UnityAudioState)
            {
                DrawDefaultInspector();
            }
        }

        // Fixup for unexpected serialization problems
        EditorUtility.SetDirty(manager);
    }
}