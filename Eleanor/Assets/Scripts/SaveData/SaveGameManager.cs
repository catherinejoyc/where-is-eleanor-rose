using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SaveGameManager : MonoBehaviour
{
    public void SaveGame(string saveName)
    {

    }

    private JObject GenerateSceneJObject()
    {
        List<ISerializable> serializableGOs = new List<ISerializable>();


        serializableGOs.AddRange(FindObjectsOfType<ISerializable>());

        JObject jObjectScene = new JObject();
        for (int i = 0; i < toSaveGOs.Count; i++)
        {
            string goJson = toSaveGOs[i].Serialize();
            JObject jGO = JObject.Parse(toSaveGOs[i].Serialize());
            jObjectScene.Add(jGO.First);
        }
        return jObjectScene;
    }

}
