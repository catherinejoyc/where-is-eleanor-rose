using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }


    public VideoPlayer currentVideoPlayer;

    private void Awake()
    {
        if (Instance == null)
            instance = this;
        else
            Debug.Log("GameManager already exists!");
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
                ContinueGame();
            else
            {
                PauseGame();
            }
        }
    }

    public void LoadLevel(int sceneID)
    {
        AudioManager.Instance.StopAllAudio();

        SceneManager.LoadScene(sceneID);
    }

    public int GetLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        return level;
    }

    bool gamePaused = false;

    public void PauseGame()
    {
        gamePaused = true;

        //hide controls
        HideControls();

        //stop footsteps
        AudioManager.Instance.SuddenstopFootsteps();

        //Activate Pause Menu Panel
        UIManager.Instance.ActivateMenu();

        //Pause Game
        Time.timeScale = 0;

        //Pause Video
        if (currentVideoPlayer != null)
            currentVideoPlayer.Pause();
    }

    public void ContinueGame()
    {
        gamePaused = false;

        //hide controls
        HideControls();

        //Continue Video
        if (currentVideoPlayer != null)
            currentVideoPlayer.Play();

        //Continue Game
        Time.timeScale = 1;

        //Deactivate Pause Menu Panel
        UIManager.Instance.DeactivateMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    bool controlsActive = false;

    public void ShowControls()
    {
        if (!controlsActive)
            UIManager.Instance.ShowControls();
    }
    public void HideControls()
    {
        UIManager.Instance.HideControls();
    }
}
