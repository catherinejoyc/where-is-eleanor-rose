using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private AK.Wwise.Bank Soundbank1;

    [Header("Soundtrack")]
    public float volumeFix;
    [SerializeField] private AK.Wwise.Event Play_Music;
    [SerializeField] private AK.Wwise.Event Stop_Music;
    [SerializeField] private AK.Wwise.Event Stop_Music_NoFade;
    [SerializeField] private AK.Wwise.State State_Start;
    [SerializeField] private AK.Wwise.RTPC PlayerLocation;

    [Header("SFX")]
    [SerializeField] private AK.Wwise.Event Play_Daisy;
    [SerializeField] private AK.Wwise.Event Play_Nazar;
    [SerializeField] private AK.Wwise.Event Play_ClearSight;
    [SerializeField] private AK.Wwise.Event Stop_ClearSight;
    [SerializeField] private AK.Wwise.RTPC Lowpass;
    [SerializeField] private AK.Wwise.Event Play_CSGlitch;
    [SerializeField] private AK.Wwise.Event Stop_CSGlitch;
    public float lowpassValue;
    [SerializeField] private AK.Wwise.Event Play_Player_Footsteps;
    [SerializeField] private AK.Wwise.Event Stop_Player_Footsteps;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("AudioManager already exists!");
    }

    void Start()
    {
        Soundbank1.Load();
        State_Start.SetValue();

        PlayMusic();

        Lowpass.SetValue(gameObject, 0);

        PlayerLocation.SetValue(gameObject, 0);
    }

    public void PlayMusic()
    {
        Play_Music.Post(gameObject);
    }
    public void FadeStopMusic()
    {
        Stop_Music.Post(gameObject);
    }
    public void SuddenStopMusic()
    {
        Stop_Music_NoFade.Post(gameObject);
    }
    public void StopAllAudio()
    {
        SuddenStopMusic();
        SuddenstopFootsteps();
        StopCSGlitch();
        StopClearSight();
    }

    #region SFX
    public void PlayDaisySound()
    {
        Play_Daisy.Post(gameObject);
    }
    public void PlayNazarSound()
    {
        Play_Nazar.Post(gameObject);
    }
    public void PlayClearSight()
    {
        Play_ClearSight.Post(gameObject);
        Lowpass.SetValue(gameObject, lowpassValue);
    }
    public void StopClearSight()
    {
        Stop_ClearSight.Post(gameObject);
        Lowpass.SetValue(gameObject, 0);
    }

    bool glitchPlaying = false;
    public void PlayCSGlitch()
    {
        if(!glitchPlaying)
        {
            glitchPlaying = true;
            Play_CSGlitch.Post(gameObject);
        }       
    }
    public void StopCSGlitch()
    {
        if(glitchPlaying)
        {
            glitchPlaying = false;
            Stop_CSGlitch.Post(gameObject);
        }
    }
    #endregion

    #region footsteps
    bool isWalking = false;
    public void PlayWalking()
    {
        if(!isWalking)
        {
            isWalking = true;
            Play_Player_Footsteps.Post(gameObject);
        }
    }
    public void StopWalking()
    {
        if(isWalking)
        {
            isWalking = false;
            Stop_Player_Footsteps.Post(gameObject);
        }
    }

    public void SuddenstopFootsteps()
    {
        Stop_Player_Footsteps.Post(gameObject);
    }
    #endregion

    public void ChangePlayerLocationValue(float location)
    {
        PlayerLocation.SetValue(gameObject, location);
    }

    #region glitch in lvl0
    [Header("Glitch in Lvl0")]
    [SerializeField] private AK.Wwise.Event PlayGlitch_BT;

    public void PlayGlitchAudio()
    {
        PlayGlitch_BT.Post(gameObject);
    }
    #endregion
}
