using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DisableVisualNovel : MonoBehaviour
{
    [Header("Set the next Dialog on PureVisualNovel")]
    public GameObject nextDialog;

    [Header("Videoplayer")]
    public float videoClipLength;
    public GameObject videoPlayerPanel;

    private void Awake()
    {
        UIManager.Instance.StopVNWithoutBlackscreen();
        PlayerScript.Instance.visualNovelMode = true;

        //Change current VideoPlayer in GameManager
        GameManager.Instance.currentVideoPlayer = videoPlayerPanel.GetComponentInChildren<VideoPlayer>();

        //Start Video
        if (graveyardScene)
            AudioManager.Instance.FadeStopMusic();
        videoPlayerPanel.SetActive(true);

        StartCoroutine(StartVisualNovelEnum());
    }

    void StartVisualNovel()
    {       
        PlayerScript.Instance.visualNovelMode = true;
        nextDialog.SetActive(true);

        videoPlayerPanel.SetActive(false);

        UIManager.Instance.visualNovelPanel.SetActive(true);
    }

    public bool graveyardScene;
    IEnumerator StartVisualNovelEnum()
    {
        Debug.LogWarning(Time.time + " - Start Video - " + videoClipLength);
        yield return new WaitForSeconds(videoClipLength);

        PlayerScript.Instance.visualNovelMode = true;

        nextDialog.SetActive(true);

        if (!graveyardScene)
            videoPlayerPanel.SetActive(false);

        UIManager.Instance.RestartVNAfterStop();

        //reset currentVP in GameManager
        GameManager.Instance.currentVideoPlayer = null;
    }
}
