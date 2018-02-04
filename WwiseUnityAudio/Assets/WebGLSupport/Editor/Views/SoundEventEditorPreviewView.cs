using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/**
 * Displays all event previews in the left column
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEventEditorPreviewView
 */
public class SoundEventEditorPreviewView
{
    private Vector2 scrollBarPosition;
    
    /**
     * Called to draw the preview
     */
    public void OnGUI()
    {
        GUILayout.Label("Events", EditorStyles.boldLabel);
        GUILayout.BeginArea(new Rect(10, 25, 400, 600));
    
        EditorGUILayout.BeginHorizontal();
    
        scrollBarPosition = EditorGUILayout.BeginScrollView(scrollBarPosition, false, true, 
            GUILayout.ExpandWidth(true), 
            GUILayout.ExpandHeight(true));
    
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
    
        if (GUILayout.Button("New event"))  { SoundEventEditorController.CreateEvent();     }
        if (GUILayout.Button("Remove all")) { SoundEventEditorController.RemoveAllEvents(); }
    
        GUILayout.Space(20);
        EditorGUILayout.EndHorizontal();
    
        GUILayout.Space(30);
        DisplayAllEventPreviews();
    
        EditorGUILayout.EndToggleGroup();
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
    }
    
    /**
     * Displays all events from the model event list
     */
    private void DisplayAllEventPreviews()
    {
        if(SoundEventEditorModel.databaseInstance == null)
        {
            return;
        }
        
        // Getting the list of sound events from the model
        List<SoundEvent> events = SoundEventEditorModel.databaseInstance.Events;
    
        if(events == null)
        {
            EditorUtility.DisplayDialog(
                "Events list null reference",
                "Unable to initializes the reference for the events list", "Ok");
        
            return;
        }

        int eventCount = events.Count;
        for (int nEvent = 0; nEvent < eventCount; ++nEvent)
        {
            SoundEvent soundEvent = events[nEvent];

            EditorGUILayout.LabelField("Event ID :",          soundEvent.EventID.ToString());
            EditorGUILayout.LabelField("Event name :",        soundEvent.EventName);
            EditorGUILayout.LabelField("Event description :", soundEvent.EventDescription);
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Edit",   GUILayout.Width(185))) { SoundEventEditorController.EditEvent(soundEvent);           }
            if (GUILayout.Button("Remove", GUILayout.Width(185))) { SoundEventEditorController.RemoveEvent(soundEvent); return; }
    
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
    
            GUILayout.Space(20);
            GUILayout.Box("", GUILayout.Width(370), GUILayout.Height(1));
            GUILayout.Space(20);
        }    
    }
}
