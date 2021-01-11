using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveGameManager : MonoBehaviour
{
    static SaveGameManager instance;
    public static SaveGameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Debug.Log("SaveGameManager already exists!");
        }
    }


    JObject _curSavegame;
    string saveName = "SaveGame";

    public void SaveGame()
    {
        string saveFilePath = Application.persistentDataPath + "/" + saveName + ".sav";

        if (_curSavegame == null)
        {
            if (File.Exists(saveFilePath) == true)
            {
                StreamReader sr = new StreamReader(saveFilePath);
                string json = sr.ReadToEnd();
                sr.Close();
                _curSavegame = JObject.Parse(json);
            }
            else
                _curSavegame = new JObject();
        }

        JObject jObjectScene = GenerateSceneJObject();
        string sceneName = SceneManager.GetActiveScene().name;

        _curSavegame["activeScene"] = sceneName;

        if (_curSavegame[sceneName] == null)
        { _curSavegame.Add(sceneName, jObjectScene); }
        else
        { _curSavegame[sceneName] = jObjectScene; }

        StreamWriter sw = new StreamWriter(saveFilePath);
        sw.WriteLine(_curSavegame.ToString());
        sw.Close();

        print("saved to: " + saveFilePath);
    }

    private JObject GenerateSceneJObject()
    {
        List<ISerializable> serializedGOs = new List<ISerializable>();


        serializedGOs.AddRange(FindObjectsOfType<SerializedObject>());

        JObject jObjectScene = new JObject();
        for (int i = 0; i < serializedGOs.Count; i++)
        {
            string goJson = serializedGOs[i].Serialize();
            JObject jGO = JObject.Parse(serializedGOs[i].Serialize());
            jObjectScene.Add(jGO.First);
        }
        return jObjectScene;
    }

    //Loads the whole savegame
    public void LoadGame()
    {
        string saveFilePath = Application.persistentDataPath + "/" + saveName + ".sav";
        print("Loading: " + saveFilePath);

        StreamReader sr = new StreamReader(saveFilePath);
        string json = sr.ReadToEnd();
        sr.Close();

        JObject jObjectSavegame = JObject.Parse(json);

        LoadScene(jObjectSavegame["activeScene"].ToString(), jObjectSavegame);
    }


    private void LoadScene(string sceneName, JObject savegame)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
            SceneManager.LoadScene(sceneName);

        StartCoroutine(LoadSavedObjects(sceneName, savegame));
    }

    private IEnumerator LoadSavedObjects(string sceneName, JObject savegame)
    {
        Debug.Log("Waiting for Scene");
        yield return new WaitWhile(() => SceneManager.GetActiveScene().name == "sceneName");


        Debug.Log("Start Loading Scene Objects");
        Dictionary<string, SerializedObject> toLoadData = new Dictionary<string, SerializedObject>();

        SerializedObject[] serializedObjects = Resources.FindObjectsOfTypeAll(typeof(SerializedObject)) as SerializedObject[];

        for (int i = 0; i < serializedObjects.Length; i++)
        {
            toLoadData.Add(serializedObjects[i]._uID, serializedObjects[i]);
        }

        JObject jObjectScene = (JObject)savegame[sceneName];
        foreach (JProperty jGO in jObjectScene.Children())
        {
            if (toLoadData.ContainsKey(jGO.Name))
            {

                toLoadData[jGO.Name].Deserialize(jGO.Value.ToString());
            }
            else
            {
                //deserialization of Objects which were created at runtime
                Debug.LogWarning("Did not deserialize " + jGO.Name + " because it is not in the scene");
            }
        }

        Debug.Log("Finished Loading Scene Objects");
    }
}
