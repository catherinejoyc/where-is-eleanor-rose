using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    public GameObject videoHoster;

    public VideoClip vidClip;

    //new approach
    public VideoPlayer videoPlayer;

    [Header("Visual Novel")]
    public bool afterSceneVisualNovel;
    public GameObject nextDialog;

    private void Awake()
    {
        //new approach
        videoPlayer.clip = vidClip;

        //old
        ActivateVideo();

        UIManager.Instance.visualNovelPanel.SetActive(false);
        PlayerScript.Instance.visualNovelMode = true;

        Invoke("DisableVideo", 2f);
    }


    void ActivateVideo()
    {
        videoHoster.SetActive(true);
    }
    void DisableVideo()
    {
        videoHoster.SetActive(false);
        //start visual novel again
        if (afterSceneVisualNovel)
        {
            UIManager.Instance.visualNovelPanel.SetActive(true);
            nextDialog.SetActive(true);
            PlayerScript.Instance.visualNovelMode = true;
        }
    }
}
