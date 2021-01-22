using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

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
    public Button resumeButton;
    Coroutine fadeInCor;

    private void Start()
    {
        // continue time 
        Time.timeScale = 1;

        //start blackScreen
        fadeInCor = StartCoroutine(fadeIn());

        //check if save game exists
        string saveFilePath = Application.persistentDataPath + "/SaveGame.sav";
        if (!File.Exists(saveFilePath))
        {
            resumeButton.interactable = false;
        }
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

    IEnumerator fadeAwayToStart(string levelname)
    {
        // loop over 1 second
        for (float i = 0; i <= 1.5f; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }


        StopAllCoroutines();

        SceneManager.LoadScene(levelname);
    }

    IEnumerator fadeAwayToResume()
    {
        // loop over 1 second
        for (float i = 0; i <= 1.5f; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }


        StopAllCoroutines();

        SaveGameManager.Instance.LoadGame();
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
        StartCoroutine(fadeAwayToStart("Level0"));
    }

    public void ResumeButtonPressed()
    {
        AudioManager.Instance.FadeStopMusic();
        StartCoroutine(fadeAwayToResume());
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
