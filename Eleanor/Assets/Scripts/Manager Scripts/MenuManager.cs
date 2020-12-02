using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //used in MainMenu

    static MenuManager instance;
    public static MenuManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (Instance == null)
            instance = this;
        else
            Debug.Log("MenuManager already exists!");
    }

    public Image blackScreen;
    Coroutine fadeInCor;

    private void Start()
    {
        // continue time 
        Time.timeScale = 1;

        //start blackScreen
        fadeInCor = StartCoroutine(fadeIn());
    }

    IEnumerator fadeIn()
    {
        for (float i = 1f; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    IEnumerator fadeAwayToStart()
    {
        // loop over 1 second
        for (float i = 0; i <= 1.5f; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }


        StopAllCoroutines();

        SceneManager.LoadScene(1);
    }

    IEnumerator fadeAwayToQuit()
    {
        // loop over 1 second
        for (float i = 0; i <= 1.5f; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }

        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartButtonPressed()
    {
        AudioManager.Instance.FadeStopMusic();
        StartCoroutine(fadeAwayToStart());
    }

    public void QuitButtonPressed()
    {
        StartCoroutine(fadeAwayToQuit());
    }

    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueButtonPressed()
    {
        GameManager.Instance.ContinueGame();
    }
}
