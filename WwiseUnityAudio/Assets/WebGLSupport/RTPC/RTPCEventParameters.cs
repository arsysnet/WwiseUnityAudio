using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

/**
 * Stores needed informations for a RTPC
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  RTPCEventParameters
 */
public class RTPCEventParameters
{
    public AudioMixerGroup   SfxMixer;
    public AudioMixerGroup   MusicMixer;
    public List<AudioSource> AudioSources = new List<AudioSource>();
}