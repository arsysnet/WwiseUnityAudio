using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/**
 * Stores informations about the events databases.
 * Loads the sound events database.
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEventEditorModel
 */
public class SoundEventEditorModel
{
    static public SoundEventDatabase databaseInstance = null;
    
    /**
     * Initializes the editor window. Loads the sound event database
     * @return False on loading error, else true
     */
    public static bool LoadSoundEventDatabase()
    {
        databaseInstance = Resources.Load("Databases/SoundEventDatabase") as SoundEventDatabase;
        if (!databaseInstance)
        {
            EditorUtility.DisplayDialog(
                "Unable to load the event database",
                "Please check if the database exist in the resources folder.", "Ok");

            return false;
        }

        return true;
    }
}
