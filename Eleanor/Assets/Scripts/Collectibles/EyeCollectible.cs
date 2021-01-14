using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCollectible : MonoBehaviour
{
    Renderer img;
    Collider2D coll;

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

        bool active = coll.isActiveAndEnabled;

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
            coll.enabled = true;
            img.enabled = true;
        }
        else
        {
            coll.enabled = false;
            img.enabled = false;
        }
    }
    #endregion

    private void Start()
    {
        img = this.GetComponent<Renderer>();
        coll = this.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.Instance.SightPoints = PlayerScript.Instance.maxSightPoints;
            Use();
        }
    }

    void Use()
    {
        AudioManager.Instance.PlayNazarSound();
        img.enabled = false;
        coll.enabled = false;
    }
}
