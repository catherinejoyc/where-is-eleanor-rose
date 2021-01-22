using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class LevelEnd : MonoBehaviour, ISerializable
{
    public int nextLevel;
    public bool endOnAwake;

    #region Save Data
    public string _json;
    public class SaveData
    {
        public bool _active;

        public SaveData(bool active)
        {
            _active = active;
        }
    }

    public string Serialize()
    {
        JObject jObj = new JObject();

        jObj.Add("componentName", GetType().Name); //script/ component name

        bool active = this.gameObject.activeSelf;

        SaveData sd = new SaveData(active);
        jObj.Add("data", JObject.Parse(JsonUtility.ToJson(sd)));

        _json = jObj.ToString();
        return _json;
    }
    public void Deserialize(string json)
    {
        JObject jObj = JObject.Parse(json);

        SaveData sd = JsonUtility.FromJson<SaveData>(jObj["data"].ToString());

        if (sd._active)
        {
            gameObject.SetActive(sd._active);
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(FadeTransition(1f));
            AudioManager.Instance.FadeStopMusic();

            //paralyze player
            if (PlayerScript.Instance != null)
            {
                PlayerScript.Instance.EndOfLevelParalysis();
            }

            AudioManager.Instance.StopAllAudio();
        }
    }

    Image blackScreen;
    IEnumerator FadeTransition(float fadeTime)
    {
        // loop over 1 second
        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            // set color with i as alpha
            blackScreen.color = new Color(0, 0, 0, i);
            yield return new WaitForFixedUpdate();
        }

        SceneManager.LoadScene(nextLevel);
    }

    private void Start()
    {
        blackScreen = UIManager.Instance.blackScreen;
    }

    private void Awake()
    {
        if (endOnAwake)
        {
            blackScreen = UIManager.Instance.blackScreen;
            StartCoroutine(FadeTransition(1f));
            AudioManager.Instance.FadeStopMusic();
        }
    }
}
