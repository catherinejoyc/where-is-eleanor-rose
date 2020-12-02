using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelEffectChanger : MonoBehaviour, IChangable
{
    public State CurrState{ get; set; }
    [Tooltip("What Post Processing Volume should be deactivated in Clear Sight? (Probably not MAIN)")]
    public PostProcessVolume postProcVol;

    public void UpdateClearState()
    {
        postProcVol.enabled = false;
    }
    public void UpdateNormalState()
    {
        postProcVol.enabled = true;
    }
}
