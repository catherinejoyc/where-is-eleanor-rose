using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class GlitchSoundLvl0 : MonoBehaviour
{
    public void Awake()
    {
        AudioManager.Instance.PlayGlitchAudio();
    }
}
