using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;

[ExecuteInEditMode]
public class SerializedObject : MonoBehaviour, ISerializable
{
    public string _uID;

    [SerializeField]
    private int instanceID = 0;

    private void Reset()
    {
        CreateNewID();
    }

    public void CreateNewID()
    {
        _uID = System.Guid.NewGuid().ToString();
    }

    public string Serialize()
    {
        List<ISerializable> serializableObj = new List<ISerializable>();
        serializableObj.AddRange(GetComponentsInChildren<ISerializable>().Where(x => (Object)x != this));

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

        gameObject.name = o["name"].ToString();


        foreach (JObject jComp in o["components"].Children())
        {
            ISerializable component = (ISerializable)GetComponent(jComp["componentName"].ToString());


            component.Deserialize(jComp.ToString());
        }
    }


}
