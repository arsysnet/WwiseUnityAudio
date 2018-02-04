using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Demo script for RTPC events
 * 
 * @author Aredhele
 * @see    https://github.com/Aredhele/WwiseUnityAudio
 * @class  FooRTPC
 */
public class FooRTPC : AbstractRTPC
{
    /**
     * Called for each RTPC events
     * 
     * @param parameters All informations about the target event
     * @param value      The target value
     */
    public override void OnRTPCUpdate(RTPCEventParameters parameters, float value)
    {
        parameters.AudioSources[0].pitch = value;
    }
}
