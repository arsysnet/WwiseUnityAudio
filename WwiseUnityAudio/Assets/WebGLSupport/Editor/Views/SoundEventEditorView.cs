using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/**
 * Displays the editor view
 * Event previews & event editions
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  SoundEventEditorView
 */
public class SoundEventEditorView : EditorWindow
{
    static public SoundEventEditorPreviewView previewView = new SoundEventEditorPreviewView();
    static public SoundEventEditorEditionView editionView = new SoundEventEditorEditionView();

    /**
     * Initializes the sound event editor window
     */
    public static void Initialize()
    {
        SoundEventEditorView window = EditorWindow.GetWindow<SoundEventEditorView>();
    
        window.titleContent.text = "Events Editor";
        window.maxSize = new Vector2(1000, 600);
        window.minSize = window.maxSize;

        float x = (Screen.currentResolution.width  - window.minSize.x) / 2.0f;
        float y = (Screen.currentResolution.height - window.minSize.y) / 2.0f;

        // Resizes / centers the window
        window.position = new Rect(x, y, window.minSize.x, window.minSize.y);
        window.Show();
    }
    
    /**
     * Renders the entire editor window from helper  view class
     */
    void OnGUI()
    {
        if(SoundEventEditorController.LoadDatabase())
        {
            this.ShowNotification(new GUIContent("Database reloaded"));
        }
    
        previewView.OnGUI();
        editionView.OnGUI();
    }
}