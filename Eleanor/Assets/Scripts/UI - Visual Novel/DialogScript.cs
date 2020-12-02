using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogScript : MonoBehaviour
{
    public string[] dialog;
    public float yOffset;
    public Sprite[] sprites;

    public Sprite background;

    public GameObject nextDialog;

    public enum MemoryFlower
    {
        Denial,
        Anger,
        Bargaining,
        Depression,
        None
    }
    public MemoryFlower flowerState = MemoryFlower.None;

    int index = 0;

    //normal speech
    void Say(string s)
    {
        string[] parts = s.Split(new string[] { ":" }, StringSplitOptions.None);
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        UIManager.Instance.Say(speech, speaker);
    }
    //finish speech
    void SayInstant(string s)
    {
        string[] parts = s.Split(new string[] { ":" }, StringSplitOptions.None);
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        UIManager.Instance.SayInstant(speech, speaker);
    }

    public void Talk()
    {
        if (index + 1 > dialog.Length)
            return;
        Say(dialog[index]);
        UIManager.Instance.ChangeCharacterSprite(sprites[index], yOffset);
        index++;
    }

    public void GoToPrevious()
    {
        index-=2;

        if (index + 1 > dialog.Length)
            return;
        SayInstant(dialog[index]);
        UIManager.Instance.ChangeCharacterSprite(sprites[index], yOffset);
        ++index;
    }

    public void FinishCurrentText()
    {
        --index;

        if (index + 1 > dialog.Length)
            return;
        SayInstant(dialog[index]);
        UIManager.Instance.ChangeCharacterSprite(sprites[index], yOffset);
        ++index;
    }

    #region Embodiment
    [Header("Embodiment")]
    public EmbodimentScript embodiment;
    public enum EmbodimentAction
    {
        PickUp,
        Place,
        None
    }
    public EmbodimentAction embodimentAction = EmbodimentAction.None;
    void EmbodimentTransformation()
    {
        embodiment.Place();
    }
    #endregion

    #region Space Pressing
    [Header("Space Pressing")]
    float spaceStartTime;
    float inactiveTime = 5f;
    #endregion

    #region Different Answerpossibilities
    [Header("Different Answerpossibilities")]
    public GameObject choices;
    bool isFinished = false;

    public GameObject nextDialogPart;
    #endregion

    #region Pure Visual Novel Scenes
    [Header("Pure Visual Novel Scenes")]
    public bool pureVisualNovelScene = false;
    #endregion

    #region Unlock Area
    [Header("Locked Area")]
    public LockedAreaScript lockedArea;
    void UnlockArea()
    {
        if (lockedArea != null)
            lockedArea.Unlock();
    }
    #endregion

    //close off
    [Header("Close off")]
    public bool lastPartOfTheDialog; //if true, disables parent object after this dialog --> FinishDialog()
    public GameObject parentObject; //ParentObject, usually named VNDialog_ParentTrigger

    private void Awake()
    {
        Debug.Log(this.gameObject.name);
    }

    private void Start()
    {
        //change background image if given in inspector
        if (background != null)
        {
            UIManager.Instance.VNbackgroundImg.overrideSprite = background;
        }

        if (pureVisualNovelScene)
        {
            PlayerScript.Instance.visualNovelMode = true;

            UIManager.Instance.StartPureVNBlackScreen(1f, Talk);

            spaceStartTime = Time.time;
        }
        else if (GetComponent<Collider2D>() == null)
            Talk();

        //TEXT CORRECTION MANAGER
        if (FindObjectOfType<TextCorrectionManager>() != null)
        {
            TextCorrectionManager.Instance.UpdateUI(this.gameObject.name);
        }
    }

    private void Update()
    {
        if (!isFinished)
        {
            if (PlayerScript.Instance.visualNovelMode)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    spaceStartTime = Time.time;
                    UIManager.Instance.DisableHelp();

                    if (!UIManager.Instance.isSpeaking || UIManager.Instance.isWaitingForUserInput)
                    {
                        if (index >= dialog.Length)
                        {
                            FinishDialog();
                            return;
                        }

                        Talk();
                    }
                    else if (UIManager.Instance.isSpeaking)
                    {
                        FinishCurrentText();
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow) && index-2 >= 0) //previous
                {
                    spaceStartTime = Time.time;
                    UIManager.Instance.DisableHelp();

                    GoToPrevious();
                }
            }

            //Help
            if (spaceStartTime + inactiveTime < Time.time && PlayerScript.Instance.visualNovelMode)
            {
                UIManager.Instance.ActivateHelp();
            }

            //Skip Dialog
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Space) && PlayerScript.Instance.visualNovelMode)
            {
                if (!UIManager.Instance.isSpeaking || UIManager.Instance.isWaitingForUserInput)
                {
                    FinishDialog();
                }
            }
        }

    }

    public void InitialDialogActivation()
    {
        PlayerScript.Instance.visualNovelMode = true;

        UIManager.Instance.StartBlackScreen(1f, Talk);

        spaceStartTime = Time.time;

        if (embodimentAction == EmbodimentAction.Place)
        {
            Invoke("EmbodimentTransformation", 1.5f);
        }
    }

    public void FinishDialog()
    {

        if (isFinished)
            return;

        Debug.LogWarning("Finish Dialog.");

        // check for choices
        if (choices == null && nextDialogPart == null)
        {
            // deactivate VN
            PlayerScript.Instance.visualNovelMode = false;

            // unlock area
            UnlockArea();

            UIManager.Instance.StartBlackScreen(1f, Talk);

            if (GetComponent<Collider2D>() != null)
                this.GetComponent<Collider2D>().enabled = false;
            else if (GetComponentInParent<Collider2D>() != null)
                GetComponentInParent<Collider2D>().enabled = false;

            //activate next dialog if exists
            if (nextDialog != null)
                nextDialog.SetActive(true);

            CheckBlackScreen();
        }
        else if (choices != null && nextDialogPart == null)
        {
            choices.SetActive(true);

            isFinished = true;
        }
        else if (choices == null && nextDialogPart != null)
        {
            nextDialogPart.SetActive(true);

            isFinished = true;
        }

        //if last dialog part, close off
        if (lastPartOfTheDialog)
        {
            parentObject.SetActive(false);

            //change back vn image 
            UIManager.Instance.VNbackgroundImg.overrideSprite = null;
        }
    }

    void CheckBlackScreen()
    {
        if (UIManager.Instance.blackScreenOn)
            Invoke("CheckBlackScreen", 1f);
        else
        {
            PostBlackScreenFinish();
        }
    }

    void PostBlackScreenFinish()
    {
        //pick up embodiment
        if (embodimentAction == EmbodimentAction.PickUp)
        {
            embodiment.PickUp();
        }

        //flowerState
        switch (flowerState)
        {
            case MemoryFlower.Denial:
                UIManager.Instance.ActivateDenial();
                break;
            case MemoryFlower.Anger:
                UIManager.Instance.ActivateAnger();
                break;
            case MemoryFlower.Bargaining:
                UIManager.Instance.ActivateBargaining();
                break;
            case MemoryFlower.Depression:
                UIManager.Instance.ActivateDepression();
                break;
        }

        //deactivate this dialog
        gameObject.SetActive(false);
        return;
    }


}
