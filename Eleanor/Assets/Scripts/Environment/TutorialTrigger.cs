using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

public class TutorialTrigger : MonoBehaviour, ISerializable
{
    public bool currentlyActive;

    public string[] dialog;
    int index = 0;
    public bool isFinished = false;

    [Tooltip("activates after this tutorial, usually the next tutorial")]
    public GameObject activateableGO;

    #region Save Data
    public string _json;
    public class SaveData
    {
        public bool _active;
        public bool _curActive;

        public SaveData(bool active, bool curActive)
        {
            _active = active;
            _curActive = curActive;
        }
    }

    public string Serialize()
    {
        JObject jObj = new JObject();

        jObj.Add("componentName", GetType().Name); //script/ component name

        bool active = (isFinished) ? false : true;
        bool curActive = currentlyActive;

        SaveData sd = new SaveData(active, curActive);
        jObj.Add("data", JObject.Parse(JsonUtility.ToJson(sd)));

        _json = jObj.ToString();
        return _json;
    }
    public void Deserialize(string json)
    {
        JObject jObj = JObject.Parse(json);

        SaveData sd = JsonUtility.FromJson<SaveData>(jObj["data"].ToString());

        currentlyActive = sd._curActive;
        this.gameObject.SetActive(sd._active);
    }
    #endregion

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

                            currentlyActive = false;

                            //save game
                            SaveGameManager.Instance.SaveGame();

                            // deactivate this
                            gameObject.SetActive(false);

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
}
