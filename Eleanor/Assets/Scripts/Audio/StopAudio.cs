using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAudio : MonoBehaviour
{
    private void Awake()
    {
        AudioManager.Instance.SuddenStopMusic();
        AudioManager.Instance.PlayGlitchAudio();
    }
}
