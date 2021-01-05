﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

[ExecuteInEditMode]
public class SerializedObject : MonoBehaviour, ISerializable
{
    public string _uID;

    [SerializeField]
    [HideInInspector]
    private int instanceID = 0;

    public void CreateNewID()
    {
        _uID = System.Guid.NewGuid().ToString();
    }

    void Awake()
    {
        if (Application.isPlaying == false)
        {
            if (instanceID != GetInstanceID())
            {
                if (instanceID == 0)
                {
                    instanceID = GetInstanceID();
                }
                else
                {
                    instanceID = GetInstanceID();
                    GetComponent<SerializedObject>().CreateNewID();
                }
            }
        }
    }

    public string Serialize()
    {
        List<ISerializable> serializableObj = new List<ISerializable>();
        serializableObj.AddRange(GetComponentsInChildren<ISerializable>());

        JObject jObj = new JObject();

        jObj.Add("name", name);

        JArray componentArray = new JArray();
        for (int i = 0; i < serializableObj.Count; i++)
        {
            ISerializable curObj = serializableObj[i];

            componentArray.Add(JObject.Parse(curObj.Serialize()));
        }
        jObj.Add("components", componentArray);

        JProperty jPropertyGO = new JProperty(_uID, jObj);
        JObject jGO = new JObject(jPropertyGO);

        return jGO.ToString();
    }

    public void Deserialize(string json)
    {
        JObject o = JObject.Parse(json);

        gameObject.name = o["goName"].ToString();


        foreach (JObject jComp in o["components"].Children())
        {
            ISerializable component = (ISerializable)GetComponent(jComp["componentName"].ToString());


            component.Deserialize(jComp.ToString());
        }
    }


}
