using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (Instance == null)
            instance = this;
        else
            Debug.Log("UIManager already exists!");
    }

    private void Start()
    {
        StartCoroutine(InitialFade());
    }

    #region daisies
    public Image[] daisySprites = new Image[3];
    public void UpdateDaisies(int val)
    {
        var tempColor = daisySprites[0].color;
        switch (val)
        {
            case 0:
                //first daisy
                tempColor.a = 0.3f;
                daisySprites[0].color = tempColor;                
                //second daisy
                tempColor.a = 0.3f;
                daisySprites[1].color = tempColor;
                //third daisy
                tempColor.a = 0.3f;
                daisySprites[2].color = tempColor;
                break;
            case 1:
                //first daisy
                tempColor.a = 1f;
                daisySprites[0].color = tempColor;
                daisySprites[0].GetComponentInChildren<ParticleSystem>().Play();
                //second daisy
                tempColor.a = 0.3f;
                daisySprites[1].color = tempColor;
                //third daisy
                tempColor.a = 0.3f;
                daisySprites[2].color = tempColor;
                break;
            case 2:
                //first daisy
                tempColor.a = 1f;
                daisySprites[0].color = tempColor;
                daisySprites[0].GetComponentInChildren<ParticleSystem>().Play();
                //second daisy
                tempColor.a = 1;
                daisySprites[1].color = tempColor;
                daisySprites[1].GetComponentInChildren<ParticleSystem>().Play();
                //third daisy
                tempColor.a = 0.3f;
                daisySprites[2].color = tempColor;
                break;
            case 3:
                //first daisy
                tempColor.a = 1f;
                daisySprites[0].color = tempColor;
                daisySprites[0].GetComponentInChildren<ParticleSystem>().Play();
                //second daisy
                tempColor.a = 1f;
                daisySprites[1].color = tempColor;
                daisySprites[1].GetComponentInChildren<ParticleSystem>().Play();
                //third daisy
                tempColor.a = 1f;
                daisySprites[2].color = tempColor;
                daisySprites[2].GetComponentInChildren<ParticleSystem>().Play();
                break;
            default:
                Debug.Log("UIManager: UpdateDaisies() - no valid value");
                break;
        }
    }
    #endregion

    #region sight points/clear sight
    public Slider sightBar;
    public Image sliderImg;
    public Text CSText;
    public Image fillImage;
    bool glitchActive;

    public GlitchEffect glitchEffect;

    public void UpdateSightBar(float val)
    {
        sightBar.value = val;
        if (sightBar.value < 5f) //low on clear sight
        {
            sliderImg.CrossFadeColor(Color.red, 1f, true, true);

            //activate glitch
            glitchActive = true;
        }
        else
        {
            sliderImg.CrossFadeColor(Color.white, 1f, true, true);

            //deactivate glitch
            glitchActive = false;
        }

    }

    public void ShowClearSightUI()
    {
        CSText.color = new Color(1,1,1,1);
        fillImage.color = new Color(1, 1, 1, 1);
    }
    #endregion

    #region visual novel
    [Header("Visual Novel")]
    public GameObject visualNovelPanel;
    public Image helpImg;
    public Image VNbackgroundImg;
    public Text dialogPartner;
    public Text dialog;
    public Image characterSprite;
    public Animator spriteAnimator;

    [Header("Name Colors")]
    public Text nameColor;
    public Color eleanorColor;
    public Color georgiaColor;
    public Color amberColor;
    public Color denialColor;
    public Color angerColor;
    public Color bargainingColor;
    public Color depressionColor;

    [Header("Textboxes")]
    public Image textboxSprite;
    public Sprite eleanorBox;
    public Sprite georgiaBox;
    public Sprite amberBox;
    public Sprite denialBox;
    public Sprite angerBox;
    public Sprite bargainingBox;
    public Sprite depressionBox;

    //help text
    public void ActivateHelp()
    {
        helpImg.CrossFadeColor(Color.white, 1f, true, true);

    }
    public void DisableHelp()
    {
        helpImg.CrossFadeColor(Color.clear, 1f, true, true);
    }

    //normal text
    public void Say(string speech, string speaker = "")
    {
        StopSpeaking();

        speaking = StartCoroutine(Speaking(speech, speaker));
    }
    //finish text
    public void SayInstant(string speech, string speaker = "")
    {
        StopSpeaking();

        visualNovelPanel.SetActive(true);

        dialogPartner.text = DetermineSpeaker(speaker);
        isWaitingForUserInput = true;

        dialog.text = speech;
    }

    public void StopSpeaking()
    {
        if (isSpeaking)
        {
            StopCoroutine(speaking);
        }
        speaking = null;
    }

    public bool isSpeaking
    {
        get
        {
            return speaking != null;
        }
    }
    public bool isWaitingForUserInput = false;

    string targetSpeech = "";
    Coroutine speaking = null;
    IEnumerator Speaking(string speech, string speaker = "")
    {
        visualNovelPanel.SetActive(true);

        targetSpeech = speech;

        dialog.text = "";

        //name
        string speakerName = DetermineSpeaker(speaker);
        dialogPartner.text = speakerName;       

        isWaitingForUserInput = false;

        while (dialog.text != targetSpeech)
        {
            dialog.text += targetSpeech[dialog.text.Length];
            yield return new WaitForFixedUpdate();
        }

        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
            yield return new WaitForFixedUpdate();

        StopSpeaking();
    }

    string DetermineSpeaker(string s)
    {
        string val = dialogPartner.text;
        if (s != dialogPartner.text && s != "")
        {
            val = s.ToLower().Contains("narrator") ? "" : s;
        }

        //change color and textbox for name
        switch (s)
        {
            case "Georgia Daisy":
                nameColor.color = georgiaColor;
                textboxSprite.sprite = georgiaBox;
                break;
            case "Eleanor Rose":
                nameColor.color = eleanorColor;
                textboxSprite.sprite = eleanorBox;
                break;
            case "Amber Summers":
                nameColor.color = amberColor;
                textboxSprite.sprite = amberBox;
                break;
            case "Cat girl":
                nameColor.color = denialColor;
                textboxSprite.sprite = denialBox;
                break;
            case "Edgy kid":
                nameColor.color = angerColor;
                textboxSprite.sprite = angerBox;
                break;
            case "A teacher???":
                nameColor.color = bargainingColor;
                textboxSprite.sprite = bargainingBox;
                break;
            case "Goth girl":
                nameColor.color = depressionColor;
                textboxSprite.sprite = depressionBox;
                break;
            case "???":
                //check Level
                switch (GameManager.Instance.GetLevel())
                {
                    case 2: //denial
                        nameColor.color = denialColor;
                        textboxSprite.sprite = denialBox;
                        break;
                    case 3: //anger
                        nameColor.color = angerColor;
                        textboxSprite.sprite = angerBox;
                        break;
                    case 4: //bargaining
                        nameColor.color = bargainingColor;
                        textboxSprite.sprite = bargainingBox;
                        break;
                    case 5: //depression
                        nameColor.color = depressionColor;
                        textboxSprite.sprite = depressionBox;
                        break;
                }
                break;
        }

        return val;
    }

    public bool ChangeCharacterSprite(Sprite newSpr, float yOffset)
    {
        if (characterSprite.sprite == newSpr || newSpr == null)
            return false;
        else
        {
            characterSprite.sprite = newSpr;

            //shake
            spriteAnimator.SetTrigger("spriteChange");
        }
        return true;
    }

    // choice panel
    public GameObject choicePanel;
    public TMPro.TMP_Text path1;
    public TMPro.TMP_Text path2;
    public Button path1Btn;
    public Button path2Btn;

    #endregion

    #region black screen
    [Header("Blackscreen")]
    public Image blackScreen;
    public bool blackScreenOn;

    IEnumerator FadeBlackScreen(float fadeTime, Action visualNovelModeMethod)
    {
        blackScreenOn = true;
        // loop over 1 second
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }

        if (PlayerScript.Instance.visualNovelMode)
        {
            visualNovelPanel.SetActive(true);
            characterSprite.enabled = true;
            visualNovelModeMethod();
        }
        else
        {
            // deactivate visual novel mode
            characterSprite.enabled = false;
            visualNovelPanel.SetActive(false);
        }

        // loop over 1 second backwards
        for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            if (i < 0.5f)
                blackScreenOn = false;
            yield return new WaitForFixedUpdate();
        }

    }

    public void StartBlackScreen(float fadeTime, Action method)
    {
        StartCoroutine(FadeBlackScreen(fadeTime, method));
    }

    public void StartPureVNBlackScreen(float fadeTime, Action method)
    {
        StartCoroutine(PureVNFadeBlackScreen(fadeTime, method));
    }

    IEnumerator PureVNFadeBlackScreen(float fadeTime, Action visualNovelModeMethod)
    {
        blackScreenOn = true;
        blackScreen.color = new Color(0, 0, 0, 1);

        if (PlayerScript.Instance.visualNovelMode)
        {
            visualNovelPanel.SetActive(true);
            characterSprite.enabled = true;
            visualNovelModeMethod();
        }
        else
        {
            // deactivate visual novel mode
            characterSprite.enabled = false;
            visualNovelPanel.SetActive(false);
        }

        // loop over 1 second backwards
        for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            if (i < 0.5f)
                blackScreenOn = false;
            yield return new WaitForFixedUpdate();
        }
    }

    public void StartInitialFade()
    { StartCoroutine(InitialFade()); }
    //Initial Fade
    IEnumerator InitialFade()
    {
        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }
    }

    //Stargazing (Stop VN without blackscreen)
    public void StopVNWithoutBlackscreen()
    {
        characterSprite.enabled = false;
        visualNovelPanel.SetActive(false);
    }
    public void RestartVNAfterStop()
    {
        characterSprite.enabled = true;
        visualNovelPanel.SetActive(true);
    }
    #endregion

    #region death
    [Header("Death")]
    public GameObject deathUpPanel;
    public Text deathUpText;
    public Text deathUpName;
    public Image deathUpBox;

    IEnumerator FadeDeathTransition(float fadeTime)
    {
        // loop over 1 second
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }

        // start death message
        StartDeathPopUp();
    }

    public void StartDeathTransition(float fadeTime)
    {
        StartCoroutine(FadeDeathTransition(fadeTime));
    }
    public void StartDeathPopUp()
    {
        deathUpPanel.SetActive(true);

        deathUpName.text = "???";

        //check level
        switch (GameManager.Instance.GetLevel())
        {
            case 2: //denial
                DeathSay("Wake up, nya!");

                deathUpName.color = denialColor;
                deathUpBox.sprite = denialBox;
                break;
            case 3: //anger
                DeathSay("Hey! Don't go to sleep now!");

                deathUpName.color = angerColor;
                deathUpBox.sprite = angerBox;
                break;
            case 4: //bargaining
                DeathSay("This is not the time for a nap.");

                deathUpName.color = bargainingColor;
                deathUpBox.sprite = bargainingBox;
                break;
            case 5: //depression
                DeathSay("... Oh no. Please wake up...");

                deathUpName.color = depressionColor;
                deathUpBox.sprite = depressionBox;
                break;
        }

    }
    void DeathSay(string s)
    {
        StartCoroutine(DeathMessage(s));
    }
    IEnumerator DeathMessage(string s)
    {
        while (deathUpText.text != s)
        {
            deathUpText.text += s[deathUpText.text.Length];
            yield return new WaitForEndOfFrame();
        }


        yield return new WaitForSeconds(3);
        PlayerScript.Instance.Respawn();
        StartCoroutine(FadebackAfterDeath());
        deathUpPanel.SetActive(false);
    }

    IEnumerator FadebackAfterDeath()
    {
        // fade black screen away
        for (float i = 1f; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }
    }

    #endregion

    #region memory flowers
    public Image denial;
    public Image anger;
    public Image bargaining;
    public Image depression;

    public void ActivateDenial()
    {
        //Sprite change
        var tempColor = denial.color;

        tempColor.a = 1f;
        denial.color = tempColor;
    }
    public void ActivateAnger()
    {
        //Sprite change
        var tempColor = anger.color;

        tempColor.a = 1f;
        anger.color = tempColor;
    }
    public void ActivateBargaining()
    {
        //Sprite change
        var tempColor = bargaining.color;

        tempColor.a = 1f;
        bargaining.color = tempColor;
    }
    public void ActivateDepression()
    {
        //Sprite change
        var tempColor = depression.color;

        tempColor.a = 1f;
        depression.color = tempColor;
    }

    #endregion

    #region pop up
    [Header("Pop Up")]
    public GameObject popUpGO;
    public Text popUpText;
    public Text popUpName;
    public Image popUpBox;
    public bool popUpActive;

    public void StartPopUp(string speech, string speaker)
    {
            
        popUpGO.SetActive(true);
        popUpText.text = speech;

        popUpName.text = speaker;

        //change color and textbox for name
        switch (speaker)
        {
            case "Georgia Daisy":
                popUpName.color = georgiaColor;
                popUpBox.sprite = georgiaBox;
                break;
            case "Eleanor Rose":
                popUpName.color = eleanorColor;
                popUpBox.sprite = eleanorBox;
                break;
            case "Amber Summers":
                popUpName.color = amberColor;
                popUpBox.sprite = amberBox;
                break;
            case "Denial":
                popUpName.color = denialColor;
                popUpBox.sprite = denialBox;
                break;
            case "Anger":
                popUpName.color = angerColor;
                popUpBox.sprite = angerBox;
                break;
            case "Bargaining":
                popUpName.color = bargainingColor;
                popUpBox.sprite = bargainingBox;
                break;
            case "Depression":
                popUpName.color = depressionColor;
                popUpBox.sprite = depressionBox;
                break;
            case "???":
                //check Level
                switch (GameManager.Instance.GetLevel())
                {
                    case 2: //denial
                        popUpName.color = denialColor;
                        popUpBox.sprite = denialBox;
                        break;
                    case 3: //anger
                        popUpName.color = angerColor;
                        popUpBox.sprite = angerBox;
                        break;
                    case 4: //bargaining
                        popUpName.color = bargainingColor;
                        popUpBox.sprite = bargainingBox;
                        break;
                    case 5: //depression
                        popUpName.color = depressionColor;
                        popUpBox.sprite = depressionBox;
                        break;
                }
                break;
        }

        popUpActive = true;
    }

    public void StopPopUp()
    {
        popUpText.text = "";

        popUpActive = false;

        popUpGO.SetActive(false);
    }
    #endregion

    #region picked up objects
    [Header("Picked up objects")]
    public Image backgroundImg;
    public Image fillImg;
    public Slider objFill;
    public Animator objAnimator;
    Sprite objImg;
    Sprite monsterImg;

    public void AddObjImage(Sprite _objImg, Sprite _monsterImg)
    {
        //set new image
        objImg = _objImg;
        monsterImg = _monsterImg;

        backgroundImg.sprite = _objImg;
        fillImg.sprite = _objImg;

        //show slider
        objFill.gameObject.SetActive(true);
        objFill.value = 1;
    }

    public void DeactivateObjImage()
    {
        //disable
        objFill.gameObject.SetActive(false);
    }
    public void UpdateObjCountdown(float countdownTime, float currPickUpTime)
    {
        float fillValue = currPickUpTime / countdownTime;
        objFill.value = 1-fillValue;

        if (objFill.value <= 0.25f) //quartal
        {
            //blink
            objAnimator.SetTrigger("Quartal");
        }
        else if (objFill.value <= 0.5f) //halftime
        {
            //blink faster
            objAnimator.SetTrigger("Halftime");
        }
    }

    #endregion

    #region menu
    [Header("Menu")]
    public GameObject menuPanel;
    public GameObject controlsPanel;
    public void ActivateMenu()
    {
        menuPanel.SetActive(true);
    }

    public void DeactivateMenu()
    {
        menuPanel.SetActive(false);
    }

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }
    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }
    #endregion

    private void Update()
    {
        #region Picked Up Obj Animator
        if (Input.GetKeyDown(KeyCode.Space))
            objAnimator.enabled = false;
        if (Input.GetKeyUp(KeyCode.Space))
            objAnimator.enabled = true;
        #endregion

        #region glitch effect
        if (glitchActive && PlayerScript.Instance.currState == State.Clear) //if low on clear sight and currently in clear sight
        {
            if (glitchEffect != null)
            {
                glitchEffect.intensity = 0.5f;
                glitchEffect.colorIntensity = 0.5f;
                glitchEffect.flipIntensity = 0.5f;

                if (sightBar.value < 2.5f) //more intense
                {
                    glitchEffect.intensity = 1;
                    glitchEffect.colorIntensity = 1f;
                    glitchEffect.flipIntensity = 1f;
                }

                //play glitch audio
                AudioManager.Instance.PlayCSGlitch();
            }
        }
        else
        {
            if (glitchEffect != null)
            {
                glitchEffect.intensity = 0;
                glitchEffect.colorIntensity = 0;
                glitchEffect.flipIntensity = 0;
            }

            AudioManager.Instance.StopCSGlitch();
        }
        #endregion
    }
}
