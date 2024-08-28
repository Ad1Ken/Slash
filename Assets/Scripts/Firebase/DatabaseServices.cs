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
    public JSONLevelData jsonLevelData;
    public LevelData levelData;

    public DatabaseReference DatabaseReference;

    public Image itemPlacedObject;
    #endregion

    #region PRIVATE_PROPERTIES
    private string levelParameters;
    private StorageManager storageManager;
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        storageManager = new StorageManager();
        //OutputJSON();
    }

    private void Start()
    {
        LoadActiveLevelData(1);
    }
    #endregion

    #region PUBLIC_METHODS
    //public void OutputJSON()
    //{
    //    string json = JsonUtility.ToJson(jsonLevelData);
    //    File.WriteAllText(Application.dataPath + "/jsonLevelData.txt", json);
    //}
    public void LoadActiveLevelData(int levelIndex)
    {
        StartCoroutine(LoadData("LevelData", () => {
            StartCoroutine(DownloadImages(levelIndex));
        }));
    }
    #endregion

    #region PRIVATE_METHODS
    private Sprite SpriteFromTexture2D(Texture2D texture) //Fucntion to return sprite through textures
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void InitializeLevelData(int levelIndex)
    {
        levelData.perLevelData = new PerLevelData[1];
        levelData.perLevelData[0] = new PerLevelData();
        levelData.perLevelData[0].levelParameters = new LevelParameters[jsonLevelData.jsonPerLevelData[levelIndex].levelParameters.Length];
        for (int j = 0; j < levelData.perLevelData[0].levelParameters.Length; j++)
        {
            levelData.perLevelData[0].levelParameters[j] = new LevelParameters();

            // Instantiate the Image directly using the prefab assigned in the inspector
            Image imageComponent = Instantiate(itemPlacedObject);

            // Assign the instantiated Image component to the levelParameters
            levelData.perLevelData[0].levelParameters[j].itemImage = imageComponent;
        }
        //for (int i = 0; i < levelData.perLevelData.Length; i++)
        //{
        //    levelData.perLevelData[i] = new PerLevelData();
        //    levelData.perLevelData[i].levelParameters = new LevelParameters[jsonLevelData.jsonPerLevelData[i].levelParameters.Length];
        //    for (int j = 0; j < levelData.perLevelData[i].levelParameters.Length; j++)
        //    {
        //        levelData.perLevelData[i].levelParameters[j] = new LevelParameters();

        //        // Instantiate the Image directly using the prefab assigned in the inspector
        //        Image imageComponent = Instantiate(itemPlacedObject);

        //        // Assign the instantiated Image component to the levelParameters
        //        levelData.perLevelData[i].levelParameters[j].itemImage = imageComponent;
        //    }
        //}
    }

    private string GetImagePath(string url)
    {
        return Path.Combine(Application.persistentDataPath, Path.GetFileName(url));
    }
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    public IEnumerator LoadData(string path, Action action = null)
    {
        var serverData = DatabaseReference.Child(path).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        DataSnapshot snapshot = serverData.Result;
        //Debug.Log(" snapshot: " + snapshot);
        string jsonData = snapshot.GetRawJsonValue();
        if (jsonData != null)
        {
            Debug.Log(" Data Found: " + jsonData);
            jsonLevelData = JsonConvert.DeserializeObject<JSONLevelData>(jsonData);
            action?.Invoke();
            //levelData = JsonUtility.FromJson<LevelData>("{\"levelParameters\":" + jsonData + "}");   
        }
        else
            Debug.Log("No Data Found");
    }
    private IEnumerator DownloadImages(int levelIndex)
    {
        InitializeLevelData(levelIndex);

        for (int j = 0; j < jsonLevelData.jsonPerLevelData[levelIndex].levelParameters.Length; j++)
        {
            string url = jsonLevelData.jsonPerLevelData[levelIndex].levelParameters[j].itemImage;
            string filename = Path.GetFileName(url);
            float percentage = jsonLevelData.jsonPerLevelData[levelIndex].levelParameters[j].percentage;

            if (storageManager.ImageExists(filename))
            {
                Texture2D cachedTexture = storageManager.LoadImage(filename, out float cachedPercentage);
                if (cachedTexture != null)
                {
                    SpriteFromTexture2D(cachedTexture);
                    levelData.perLevelData[0].levelParameters[j].itemImage.sprite = SpriteFromTexture2D(cachedTexture);
                    levelData.perLevelData[0].levelParameters[j].percentage = cachedPercentage;
                }
            }
            else
            {
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(request);
                        storageManager.SaveImage(filename, texture.EncodeToPNG(), percentage);
                        levelData.perLevelData[0].levelParameters[j].itemImage.sprite = SpriteFromTexture2D(texture);
                        levelData.perLevelData[0].levelParameters[j].percentage = percentage;
                    }
                    else
                    {
                        Debug.LogError("Failed to download image: " + request.error);
                    }
                }
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
