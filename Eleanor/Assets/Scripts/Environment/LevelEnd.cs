using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    public int nextLevel;
    public bool endOnAwake;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeTransition(1f));
            AudioManager.Instance.FadeStopMusic();

            //paralyze player
            if (PlayerScript.Instance != null)
            {
                PlayerScript.Instance.EndOfLevelParalysis();
            }

            AudioManager.Instance.StopAllAudio();
        }
    }

    Image blackScreen;
    IEnumerator FadeTransition(float fadeTime)
    {
        // loop over 1 second
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }

        SceneManager.LoadScene(nextLevel);
    }

    private void Start()
    {
        blackScreen = UIManager.Instance.blackScreen;
    }

    private void Awake()
    {
        if (endOnAwake)
        {
            blackScreen = UIManager.Instance.blackScreen;
            StartCoroutine(FadeTransition(1f));
            AudioManager.Instance.FadeStopMusic();
        }
    }
}
