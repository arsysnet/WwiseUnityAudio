#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Displays the edition window to edit
 * sound events
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEventEditorEditionView
 */
public class SoundEventEditorEditionView
{
    /**
     * Stores the state of the edition window
     */
    public enum EditionState
    {
        Edition,
        Creation
    }
    
    private EditionState state;
    private SoundEvent   sourceEvent;
    private SoundEvent   currentEvent;
    private Vector2      scrollBarPosition;

    /**
     * Constructor. Sets the state to Creation
     */
    public SoundEventEditorEditionView()
    {
        ToogleCreation();
    }
    
    /**
     * Puts the editor in creation mode
     */
    public void ToogleCreation()
    {
        sourceEvent  = null;
        currentEvent = new SoundEvent();

        state = EditionState.Creation;
    }
    
    /**
     * Puts the editor in edition mode
     */
    public void ToogleEdition(SoundEvent soundevent)
    {
        sourceEvent  = soundevent;
        currentEvent = new SoundEvent(soundevent);

        state = EditionState.Edition;
    }
    
    /**
     * Displays the edition area
     */
    public void OnGUI()
    {
        // Displays events properties
        EditorGUILayout.BeginHorizontal();
        GUILayout.BeginArea(new Rect(420, 10, 560, 250));
        GUILayout.Label("Edition", EditorStyles.boldLabel);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("General");
        GUILayout.Space(5);

        // General section : ID, Name, Description
        currentEvent.EventID          = currentEvent.EventName.GetHashCode();
        EditorGUILayout.LabelField("Event ID :\t\t     " + currentEvent.EventID.ToString(), GUILayout.Width(530));
        currentEvent.EventName        = EditorGUILayout.TextField("Event Name",        currentEvent.EventName,        GUILayout.Width(530));
        currentEvent.EventDescription = EditorGUILayout.TextField("Event Description", currentEvent.EventDescription, GUILayout.Width(530));

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Settings");
        GUILayout.Space(5);

        currentEvent.EventAction = (SoundEvent.EEventAction)EditorGUILayout.EnumPopup(currentEvent.EventAction);

        // Play event
        if (currentEvent.EventAction == SoundEvent.EEventAction.Play)
        {
            currentEvent.EventType = (SoundEvent.EEventType)EditorGUILayout.EnumPopup(currentEvent.EventType);

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add target",    GUILayout.Width(278))) { currentEvent.EventTargets.Add(null); }
            if (GUILayout.Button("Remove target", GUILayout.Width(278)))
            {
                if(currentEvent.EventTargets.Count > 0)
                {
                    currentEvent.EventTargets.RemoveAt(currentEvent.EventTargets.Count - 1);
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Targets count : " + currentEvent.EventTargets.Count);

            GUILayout.EndArea();
            EditorGUILayout.EndHorizontal();

            // Displays target list
            EditorGUILayout.BeginHorizontal();
            GUILayout.BeginArea(new Rect(420, 240, 560, 100));

            scrollBarPosition = GUILayout.BeginScrollView(scrollBarPosition, false, true, GUIStyle.none, GUI.skin.verticalScrollbar);

            int targetCount = currentEvent.EventTargets.Count;
            for (int nTarget = 0; nTarget < targetCount; ++nTarget)
            {
                currentEvent.EventTargets[nTarget] = EditorGUILayout.ObjectField("Target " + nTarget.ToString(),
                    currentEvent.EventTargets[nTarget], typeof(AudioClip), false) as AudioClip;
            
                GUILayout.Space(1);
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndArea();
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(420, 350, 560, 250));
            
            currentEvent.EventMaxInstance = EditorGUILayout.IntField("Max instances", currentEvent.EventMaxInstance, GUILayout.Width(530));
            currentEvent.EventVolume = EditorGUILayout.Slider(new GUIContent("Volume"), currentEvent.EventVolume,  0.0f, 1.0f, GUILayout.Width(530));
            currentEvent.EventPitch  = EditorGUILayout.Slider(new GUIContent("Pitch"),  currentEvent.EventPitch,  -3.0f, 3.0f, GUILayout.Width(530));

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            currentEvent.EventIsLooping      = EditorGUILayout.Toggle(new GUIContent("Loop"),           currentEvent.EventIsLooping,      GUILayout.Width(360));
            currentEvent.EventIsRandom       = EditorGUILayout.Toggle(new GUIContent("Random"),         currentEvent.EventIsRandom,       GUILayout.Width(360));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            currentEvent.EventIsVolumeRandom = EditorGUILayout.Toggle(new GUIContent("Random volume"),  currentEvent.EventIsVolumeRandom, GUILayout.Width(360));
            currentEvent.EventIsPitchRandom  = EditorGUILayout.Toggle(new GUIContent("Random pitch"),   currentEvent.EventIsPitchRandom,  GUILayout.Width(360));
            EditorGUILayout.EndHorizontal();

            // Random ranges
            GUILayout.Space(10);
            if (currentEvent.EventIsVolumeRandom)
            {
                currentEvent.EventVolumeRandomRange = EditorGUILayout.Vector2Field(new GUIContent("Volume range offset"), currentEvent.EventVolumeRandomRange);
            }
            if(currentEvent.EventIsPitchRandom)
            {
                currentEvent.EventPitchRandomRange = EditorGUILayout.Vector2Field(new GUIContent("Pitch range offset"), currentEvent.EventPitchRandomRange);
            }

            GUILayout.EndArea();

        }
        // Stop event
        else if(currentEvent.EventAction == SoundEvent.EEventAction.Stop)
        {
            currentEvent.EventToStop = EditorGUILayout.TextField("Target event", currentEvent.EventToStop, GUILayout.Width(530));

            GUILayout.EndArea();
            EditorGUILayout.EndHorizontal();
        }
        // Rtpc event
        else if(currentEvent.EventAction == SoundEvent.EEventAction.Rtpc)
        {
            currentEvent.EventToStop = EditorGUILayout.TextField("Target event", currentEvent.EventToStop, GUILayout.Width(530));

            // RTPC object
            currentEvent.EventRTPC = EditorGUILayout.ObjectField("RTPC object", currentEvent.EventRTPC, typeof(AbstractRTPC), true) as AbstractRTPC;

            GUILayout.EndArea();
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.EndArea();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.BeginArea(new Rect(420, 570, 560, 50));
        if (state == EditionState.Creation)
        {
            if (GUILayout.Button("Create"))
            {
                SoundEventEditorController.AddEvent(currentEvent);
                ToogleCreation();
            }
        }
        else
        {
            if (GUILayout.Button("Update"))
            {
                SoundEventEditorController.UpdateEvent(sourceEvent, currentEvent);
                ToogleCreation();
            }
        }
        GUILayout.EndArea();
    }
}
 
 