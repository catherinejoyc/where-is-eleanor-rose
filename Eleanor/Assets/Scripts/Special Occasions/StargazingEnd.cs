using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StargazingEnd : MonoBehaviour
{
    public Camera mainCamera;
    public float movementSpeed;
    public Transform goal;
    public float waitSeconds;

    bool ending = false;

    [Header("Change Level")]
    Image blackScreen;
    public int nextLevel;

    private void Awake()
    {
        //disable VN
        UIManager.Instance.StopVNWithoutBlackscreen();

        blackScreen = UIManager.Instance.blackScreen;
    }

    private void Update()
    {
        //move up
        mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, goal.position, movementSpeed * Time.deltaTime);

        if (mainCamera.transform.position == goal.position && ending == false) //goal reached
        {
            ending = true;

            //wait for x seconds, then go back
            Invoke("LevelEnd", waitSeconds);
        }
    }

    void LevelEnd()
    {
        StartCoroutine(FadeTransition(2f));
        AudioManager.Instance.FadeStopMusic();
    }

    IEnumerator FadeTransition(float fadeTime)
    {
        // loop over 1 second
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }

        SceneManager.LoadScene(nextLevel);
    }
}
