using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Abstract class to allow user defined RTPC events
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  AbstractRTPC
 */
public abstract class AbstractRTPC : MonoBehaviour
{
    /**
     * Called after each call of SetRTPCValue()
     * 
     * @param parameters This the parameter pack of the RTPC event. It contains
     * all audio source linked to the event and the two main audio mixer
     * @param value This is the value passed to SetRTPCValue()
     */
    public abstract void OnRTPCUpdate(RTPCEventParameters parameters, float value);
}