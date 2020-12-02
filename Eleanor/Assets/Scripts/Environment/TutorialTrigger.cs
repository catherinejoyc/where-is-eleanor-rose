using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialTrigger : MonoBehaviour
{
    bool currentlyActive;

    public string[] dialog;
    int index = 0;
    bool isFinished = false;

    [Tooltip("activates after this tutorial, usually the next tutorial")]
    public GameObject activateableGO;

    void Say(string s)
    {

        string[] parts = s.Split(new string[] { ":" }, StringSplitOptions.None);
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        UIManager.Instance.StartPopUp(speech, speaker);

        ++index;
    }

    private void Update()
    {
        if (!isFinished)
        {
            if (PlayerScript.Instance.tutorialMode)
            {
                if (Input.GetKeyDown(KeyCode.Space)|| Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (currentlyActive)
                    {
                        if (index >= dialog.Length)
                        {
                            UIManager.Instance.StopPopUp();
                            PlayerScript.Instance.tutorialMode = false;
                            isFinished = true;

                            //activate
                            if (activateableGO != null)
                                activateableGO.SetActive(true);

                            return;
                        }

                        Say(dialog[index]);
                    }
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            UIManager.Instance.StopPopUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentlyActive = true;
            if (!isFinished)
            {
                if (dialog.Length > 0)
                {
                    PlayerScript.Instance.tutorialMode = true;
                    Say(dialog[0]);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isFinished)
        {
            UIManager.Instance.StopPopUp();
            currentlyActive = false;

            // deactivate this
            gameObject.SetActive(false);
        }
    }
}
