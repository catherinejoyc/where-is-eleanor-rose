using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level0End : MonoBehaviour
{
    public int nextLevel;
    public AK.Wwise.Event Stop_Music;
    public GameObject audiomanager;

    private void Awake()
    {
        //paralyze player
        if (PlayerScript.Instance != null)
        {
            PlayerScript.Instance.EndOfLevelParalysis();
        }

        Stop_Music.Post(audiomanager);
        AudioManager.Instance.StopWalking();
        StartCoroutine(FadeTransition(5));
    }

    IEnumerator FadeTransition(float fadeTime)
    {
        // loop over 1 second
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            UIManager.Instance.blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }

        SceneManager.LoadScene(nextLevel);
    }
}
