using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class DatabaseServices : MonoBehaviour
{
    #region PUBLIC_PROPERTIES

    #endregion

    #region PRIVATE_PROPERTIES
    private string levelParameters;
    private StorageManager storageManager;
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        //OutputJSON();
    }

    private void Start()
    {
        storageManager = new StorageManager();
        //LoadActiveLevelData(1);
    }
    #endregion

    #region PUBLIC_METHODS
    //public void OutputJSON()
    //{
    //    string json = JsonUtility.ToJson(jsonLevelData);
    //    File.WriteAllText(Application.dataPath + "/jsonLevelData.txt", json);
    //}
    public void LoadActiveLevelData()
    {
        StartCoroutine(LoadData("LevelData"));
    }
    #endregion

    #region PRIVATE_METHODS
    private string GetImagePath(string url)
    {
        return Path.Combine(Application.persistentDataPath, Path.GetFileName(url));
    }

    private Sprite SpriteFromTexture2D(Texture2D texture) //Fucntion to return sprite through textures
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    public IEnumerator LoadData(string path)
    {
        var serverData = FirebaseManager.Instance.DatabaseReference.Child(path).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        DataSnapshot snapshot = serverData.Result;
        //Debug.Log(" snapshot: " + snapshot);
        string jsonData = snapshot.GetRawJsonValue();
        if (jsonData != null)
        {
            Debug.Log(" Data Found: " + jsonData);
            SlashManager.Instance.jsonLevelData = JsonConvert.DeserializeObject<JSONLevelData>(jsonData);
            //levelData = JsonUtility.FromJson<LevelData>("{\"levelParameters\":" + jsonData + "}");   
        }
        else
            Debug.Log("No Data Found");
    }
    

    public IEnumerator SendRequestToDownloadImage(string url, string filename, float percentage, int val)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                storageManager.SaveImage(filename, texture.EncodeToPNG(), percentage);
                SlashManager.Instance.levelData.perLevelData[0].levelParameters[val].itemImage.sprite = SpriteFromTexture2D(texture);
                SlashManager.Instance.levelData.perLevelData[0].levelParameters[val].percentage = percentage;
            }
            else
            {
                Debug.LogError("Failed to download image: " + request.error);
            }
        }
    }
        #endregion
}



    #region JSON Data
    // JSON classes to retrieve level data
    [System.Serializable]
public class JSONLevelData // All Level Data
{
    public JSONPerLevelData[] jsonPerLevelData;
}
[System.Serializable]
public class JSONPerLevelData //Each Level Data
{
    public string level;
    public JSONLevelParameters[] levelParameters; 
}

[System.Serializable]
public class JSONLevelParameters //Each Round Data
{
    public string itemImage;
    public float percentage;
}
#endregion



// Classes to populate data
[System.Serializable]
public class LevelData
{
    public PerLevelData[] perLevelData;
}
[System.Serializable]
public class PerLevelData
{
    public string level;
    public LevelParameters[] levelParameters;
}

[System.Serializable]
public class LevelParameters
{
    public Image itemImage;
    public float percentage;
}
