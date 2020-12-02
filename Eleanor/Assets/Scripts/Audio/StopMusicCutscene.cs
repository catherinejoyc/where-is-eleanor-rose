using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusicCutscene : MonoBehaviour
{
    public float videoLength;
    private void Awake()
    {
        AudioManager.Instance.FadeStopMusic();

        Invoke("PlayMusic", videoLength);
    }

    void PlayMusic()
    {
        AudioManager.Instance.PlayMusic();
    }
}
