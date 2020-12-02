using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level0Manager : MonoBehaviour
{
    public Image logo;

    public Camera mainCam;
    bool moveCam = false;

    [Header("Visual Novel")]
    public float waitForFade;
    public Transform cameraGoal;
    public GameObject dialog1;

    [Header("Videoplayer")]
    public float videoClipLength;
    public GameObject videoPlayer;
    public RawImage videoImage;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("VisualNovelFadeMethod", videoClipLength);
    }

    void CallSecondFade()
    {
        StartCoroutine(SecondFade());
    }

    IEnumerator SecondFade()
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            logo.color = new Color(i, i, i, i);
            yield return null;
        }

        //disable logo
        logo.enabled = false;
    }

    void VisualNovelFadeMethod()
    {
        dialog1.SetActive(true);
        videoPlayer.SetActive(false);

        //reset currentVP in GameManager
        GameManager.Instance.currentVideoPlayer = null;
    }
}
