using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateLocationSetter : MonoBehaviour, ISerializable
{
    public GameObject audioTriggers;
    private bool audioTriggerDeactivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            audioTriggers.SetActive(false);
            audioTriggerDeactivated = true;
        }
    }

    #region Save Data
    public string _json;
    public class SaveData
    {
        public bool _audioTriggerDeactivated;

        public SaveData(bool status)
        {
            _audioTriggerDeactivated = status;
        }
    }

    public string Serialize()
    {
        JObject jObj = new JObject();

        jObj.Add("componentName", GetType().Name); //script/ component name

        SaveData sd = new SaveData(audioTriggerDeactivated);
        jObj.Add("data", JObject.Parse(JsonUtility.ToJson(sd)));

        _json = jObj.ToString();
        return _json;
    }
    public void Deserialize(string json)
    {
        JObject jObj = JObject.Parse(json);

        SaveData sd = JsonUtility.FromJson<SaveData>(jObj["data"].ToString());

        audioTriggers.SetActive(!sd._audioTriggerDeactivated);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 5)
        {
            AudioManager.Instance.ChangePlayerLocationValue(sd._audioTriggerDeactivated ? 100 : 0);
        }
        else
        {
            AudioManager.Instance.ChangePlayerLocationValue(sd._audioTriggerDeactivated ? 60 : 0);
        }
    }
    #endregion
}

