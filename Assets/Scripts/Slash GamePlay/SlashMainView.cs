using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SlashMainView : MonoBehaviour
{
    #region  PUBLIC_PROPERTIES
    public ImageHandler imageHandler;
    public Coroutine splitCoroutine;
    public ScoreSlider scoreSlider;

    #endregion

    #region PRIVATE_PROPERTIES
    private StorageManager storageManager;
    [SerializeField] private Image itemPlacedPrefab;
    #endregion

    #region UNITY_CALLBACKS
    private void Start()
    {
        storageManager = new StorageManager();
        //Debug.Log("After/Before Firebase");
        //FirebaseManager.Instance.DatabaseServices.LoadActiveLevelData();
        //DownloadImages(1);
        StartCoroutine(StartExecution());
    }
    #endregion

    #region PUBLIC_METHODS
    public void SplitImageRoutine(float percentage)
    {
        if (splitCoroutine != null) StopCoroutine(splitCoroutine); 
            splitCoroutine = StartCoroutine(SplitImage(percentage));
    }
    #endregion

    #region PRIVATE_METHODS
    private void DownloadImages(int levelIndex)
    {
        InitializeLevelData(levelIndex);

        for (int j = 0; j < SlashManager.Instance.jsonLevelData.jsonPerLevelData[levelIndex].levelParameters.Length; j++)
        {
            string url = SlashManager.Instance.jsonLevelData.jsonPerLevelData[levelIndex].levelParameters[j].itemImage;
            string filename = Path.GetFileName(url);
            float percentage = SlashManager.Instance.jsonLevelData.jsonPerLevelData[levelIndex].levelParameters[j].percentage;

            if (storageManager.ImageExists(filename))
            {
                Texture2D cachedTexture = storageManager.LoadImage(filename, out float cachedPercentage);
                if (cachedTexture != null)
                {
                    SpriteFromTexture2D(cachedTexture);
                    SlashManager.Instance.levelData.perLevelData[0].levelParameters[j].itemImage.sprite = SpriteFromTexture2D(cachedTexture);
                    SlashManager.Instance.levelData.perLevelData[0].levelParameters[j].percentage = cachedPercentage;
                }
            }
            else
            {
                StartCoroutine(FirebaseManager.Instance.DatabaseServices.SendRequestToDownloadImage(
                    url, filename, percentage, j
                    ));
                
            }
        }
    }
    private void InitializeLevelData(int levelIndex)
    {
        SlashManager.Instance.levelData.perLevelData = new PerLevelData[1];
        SlashManager.Instance.levelData.perLevelData[0] = new PerLevelData();
        SlashManager.Instance.levelData.perLevelData[0].levelParameters = new LevelParameters[SlashManager.Instance.jsonLevelData.jsonPerLevelData[levelIndex].levelParameters.Length];
        for (int j = 0; j < SlashManager.Instance.levelData.perLevelData[0].levelParameters.Length; j++)
        {
            SlashManager.Instance.levelData.perLevelData[0].levelParameters[j] = new LevelParameters();

            // Instantiate the Image directly using the prefab assigned in the inspector
            Image imageComponent = Instantiate(itemPlacedPrefab);

            // Assign the instantiated Image component to the levelParameters
            SlashManager.Instance.levelData.perLevelData[0].levelParameters[j].itemImage = imageComponent;
        }

    }

    private Sprite SpriteFromTexture2D(Texture2D texture) //Fucntion to return sprite through textures
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines

    IEnumerator StartExecution()
    {
        FirebaseManager.Instance.InitializeFirebase();
        yield return new WaitForSeconds(3f);
        FirebaseManager.Instance.DatabaseServices.LoadActiveLevelData();
        yield return new WaitForSeconds(5f);
        DownloadImages(1);
    }
    public IEnumerator SplitImage(float percentage)
    {
        //imageHandler.originalImage.gameObject.SetActive(false);  
        imageHandler.axeAnimator.Play("Axe"); 
        yield return new WaitForSeconds(1f);
        imageHandler.SplitAndPositionImages(percentage);
        //imageHandler.SplitImage(percentage);
    }
    #endregion
}
