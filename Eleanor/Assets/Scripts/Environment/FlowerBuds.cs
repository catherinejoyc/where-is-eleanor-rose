using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBuds : MonoBehaviour, ISerializable
{
    public ParticleSystem ps;
    public Sprite buds;
    public Sprite flowers;
    public Collider2D bodyColl;

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

        SaveData sd = new SaveData(bodyColl.enabled);
        jObj.Add("data", JObject.Parse(JsonUtility.ToJson(sd)));

        _json = jObj.ToString();
        return _json;
    }
    public void Deserialize(string json)
    {
        JObject jObj = JObject.Parse(json);

        SaveData sd = JsonUtility.FromJson<SaveData>(jObj["data"].ToString());

        if(!sd._active)
        {
            //change appearance
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.sprite = flowers;
            }

            //deactivate bodyColl
            bodyColl.enabled = false;

            //deactivate self
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            //change appearance
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.sprite = buds;
            }

            foreach (Collider2D coll in GetComponents<Collider2D>())
            {
                coll.enabled = true;
            }
        }
    }
    #endregion

    void Bloom()
    {
        ps.Play();

        //add Clear Points
        PlayerScript.Instance.SightPoints = PlayerScript.Instance.maxSightPoints;

        //change appearance
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sprite = flowers;
        }

        //deactivate bodyColl
        bodyColl.enabled = false;

        //remove from UI
        UIManager.Instance.DeactivateObjImage();

        //remove from player
        PlayerScript.Instance.pickedUpObj = null;

        //deactivate self
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            //player has picked up a minion
            if (collision.GetComponent<PlayerScript>().pickedUpObj != null)
            {
                //set off dying in that minion
                collision.GetComponent<PlayerScript>().pickedUpObj.transform.position = this.transform.position;
                bool blooming = collision.GetComponent<PlayerScript>().pickedUpObj.Die();

                if (blooming)
                    Bloom();              
            }
        }
    }
}
