using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/**
 * Custom editor to create sound events with
 * the Unity Audio Engine to replace Wwise
 * to build with WebGL.
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEventEditorController
 */
public class SoundEventEditorController
{
    private static bool databaseLoaded = false;

    /**
     * Initializes the editor window. Loads the events database
     */
    [MenuItem("WebGL/Sound Event Editor")]
    static void Initialize()
    {
        LoadDatabase();
        if(databaseLoaded)
        {
            SoundEventEditorView.Initialize();
        }
    }

    /**
     * Displays the creation menu
     */
    public static void CreateEvent()
    {
        SoundEventEditorView.editionView.ToogleCreation();
    }

    /**
     * Enables the edition of the given event
     * @param soundEvent The event to edit
     */
    public static void EditEvent(SoundEvent soundEvent)
    {
        SoundEventEditorView.editionView.ToogleEdition(soundEvent);
    }

    /**
     * Removes an event from the database. Rebuilds index.
     * @param soundEvent The event to remove
     */
    public static void RemoveEvent(SoundEvent soundEvent)
    {
        List<SoundEvent> events = SoundEventEditorModel.databaseInstance.Events;
    
        if (events.Contains(soundEvent))
        {
            events.Remove(soundEvent);
        }
    }
    
    /**
     * Removes all entries from the database
     */
    public static void RemoveAllEvents()
    {
        SoundEventEditorModel.databaseInstance.Events.Clear();
    }

    /**
     * Updates an event from another one
     * @param old   The old event
     * @param other The new event
     */
    public static void UpdateEvent(SoundEvent old, SoundEvent other)
    {
        List<SoundEvent> events = SoundEventEditorModel.databaseInstance.Events;
        if (events.Contains(old))
        {
            int index = events.IndexOf(old);
            events[index] = other;
        }
    }

    /**
     * Adds a new event in the database
     * @param soundEvent The event to add in the database
     */
    public static void AddEvent(SoundEvent soundEvent)
    {
        List<SoundEvent> events = SoundEventEditorModel.databaseInstance.Events;
        events.Add(soundEvent);
    }

    /**
     * Loads the database
     */
    public static bool LoadDatabase()
    {
        if(SoundEventEditorModel.databaseInstance == null)
        {
            databaseLoaded = SoundEventEditorModel.LoadSoundEventDatabase();
            
            // The database has been reloaded
            return true;
        }

        // The database was already reloaded
        return false; 
    }
}
