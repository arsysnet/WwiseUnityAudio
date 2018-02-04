using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Demo script that shows use cases of the
 * Audio Event Manager
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  FooScript
 */
public class FooScript : MonoBehaviour
{
    private float currentPitch = 1.0f;

    /**
     * Monobehavior Start
     */
    void Start ()
    {
        AudioEventManager.PostEvent("Music_Play");
        StartCoroutine("MusicDelayer");
    }

    /**
     * Monobehavior Update
     */
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        currentPitch += x * 0.1f * Time.deltaTime;

        AudioEventManager.SetRTPCValue("Music_Pitch", currentPitch);
    }

    /**
     * Stops the music after 10 seconds
     */
    private IEnumerator MusicDelayer()
    {
        yield return new WaitForSeconds(120.0f);
        AudioEventManager.PostEvent("Music_Stop");
    }
}
