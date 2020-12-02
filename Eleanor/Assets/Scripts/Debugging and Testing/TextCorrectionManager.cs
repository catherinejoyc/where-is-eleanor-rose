using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextCorrectionManager : MonoBehaviour
{
    private static TextCorrectionManager instance;
    public static TextCorrectionManager Instance
    {
        get
        {
            return instance;
        }
    }

    string currentLevel;
    int currentLevelIndex;
    string currentDialog;
    public Text infoBox;
    public GameObject[] dialogs;


    private void Awake()
    {
        if (Instance == null)
            instance = this;
        else
            Debug.Log("Text Correction Manager already exists.");
    }

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().name;
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        infoBox.text = "Scene: " + currentLevel;
    }

    public void UpdateUI(string dialogName)
    {
        currentDialog = dialogName;
        infoBox.text = "Scene: " + currentLevel + ", Dialog: " + currentDialog;
    }

    private void Update()
    {
        //short cut to activate next dialog, only activateable if VN is not active
        if (Input.GetKeyUp(KeyCode.N))
        {
            int i = SceneManager.GetActiveScene().buildIndex;
            if (i > 2 && i < 6) //only works in certain levels
            {
                if (!PlayerScript.Instance.visualNovelMode)
                {
                    foreach (GameObject go in dialogs)
                    {
                        //if active, jump to that position
                        if (go.activeSelf)
                        {
                            PlayerScript.Instance.transform.position = go.transform.position;
                            break;
                        }
                    }
                }
            }
        }

        //changing Levels
        if (Input.GetKeyUp(KeyCode.J)) //go back
            SceneManager.LoadScene(currentLevelIndex - 1);
        if (Input.GetKeyUp(KeyCode.K)) //restart current Level
            SceneManager.LoadScene(currentLevelIndex);
        if (Input.GetKeyUp(KeyCode.L)) //next Level
            SceneManager.LoadScene(currentLevelIndex + 1);
    }
}
