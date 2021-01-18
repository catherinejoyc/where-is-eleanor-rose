using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Newtonsoft.Json.Linq;

public class LockedAreaScript : MonoBehaviour, ISerializable
{ 
    public string message;
    public string speaker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.StartPopUp(message, speaker);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.StopPopUp();
        }
    }

    // --- unlock
    public Collider2D bodyColl;
    public Collider2D triggerColl;

    //deactivate the colliders
    public void Unlock()
     {
        bodyColl.enabled = false;
        triggerColl.enabled = false;

        //black covers
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    #region save data
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

        SaveData sd = new SaveData((bodyColl.enabled));
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
            bodyColl.enabled = true;
            triggerColl.enabled = true;
        }
        else
        {
            bodyColl.enabled = false;
            triggerColl.enabled = false;

            //black covers
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
