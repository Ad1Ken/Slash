using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SlashMainView : BaseView
{
    #region  PUBLIC_PROPERTIES
    public ImageHandler imageHandler;
    public ScoreSlider scoreSlider;
    public GameObject starAnimationObj;
    #endregion

    #region PRIVATE_PROPERTIES
    private Vector2 starAnimationObjPos;
    private Coroutine splitCoroutine;
    private StorageManager storageManager;
    [SerializeField] private Image itemPlacedPrefab;
    [SerializeField] private TextMeshProUGUI percentageText;
    #endregion

    #region UNITY_CALLBACKS
    private void Start()
    {
        //storageManager = new StorageManager();
        //Debug.Log("After/Before Firebase");
        //FirebaseManager.Instance.DatabaseServices.LoadActiveLevelData();
        //DownloadImages(1);
        //starAnimationObjPos = starAnimationObj.transform.position;
    }
    #endregion

    #region PUBLIC_METHODS
    public void SetPercentageText(float percentage)
    {
        percentageText.text = percentage.ToString();
    }
    public void SetImageToSlash(Sprite imageToSlash)
    {
        imageHandler.SetOriginalImage(imageToSlash);
    }
    public void StartDownloadImageRoutine(int currentLevel)
    {
        if (storageManager == null) 
            storageManager = new StorageManager();
        StartCoroutine(DownloadImagesRoutine(currentLevel));
    }
    public void SplitImageRoutine(float percentage) //Starts the Split Image Routine
    {
        if (splitCoroutine != null) StopCoroutine(splitCoroutine); 
            splitCoroutine = StartCoroutine(SplitImage(percentage));
    }



    public void OnClickHome() //Home button
    {
        StopAllCoroutines();
        SlashUIManager.instance.HideAllPanel();
        SlashUIManager.instance.panelActivityView.ShowView();
    }
    public void OnClickExit() //Exit button
    {
        Application.Quit(); 
    }

    public void SliderStarAnimation() // This is the star Scale and position animation  
    {
        starAnimationObj.transform.localScale = Vector2.zero;
        starAnimationObj.transform.localPosition = Vector2.zero;
        starAnimationObj.SetActive(true);
        StartCoroutine(DoScaleAnimation(starAnimationObj.transform, new Vector2(4, 4), 0f, 1f, () => {
            StartCoroutine(DoPositionAnimation(starAnimationObj.transform, scoreSlider.scoreSlider.gameObject.transform.localPosition, 0f, 0.5f, () => { }));
            StartCoroutine(DoScaleAnimation(starAnimationObj.transform, new Vector2(0, 0), 0f, 0.5f, () => {
                //BrainTrainUIManagerNew.Instance.TouchBlock(false);
               starAnimationObj.gameObject.SetActive(false);
            }));
        }));
    }
    #endregion

    #region PRIVATE_METHODS
    private void DownloadImages(int levelIndex) // This downloads the images according to the currentlevel
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

    // This Initializes the LevelData Object and Instantiate images prefab, which will be used to the place the images after downloading
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

    IEnumerator DownloadImagesRoutine(int currentLevel)
    {
        yield return new WaitForSeconds(1f);
        DownloadImages(currentLevel);
    }
    public IEnumerator SplitImage(float percentage)// Coroutine to Split the image 
    {
        //imageHandler.originalImage.gameObject.SetActive(false);  
        imageHandler.axeAnimator.Play("Axe"); 
        yield return new WaitForSeconds(1f);
        imageHandler.SplitAndPositionImages(percentage);
        //imageHandler.SplitImage(percentage);
    }
    public IEnumerator UpdateScoreAnim()//Coroutine to Update Score, placing the star and updating the slider
    {
        yield return new WaitForSeconds(1f);
        SliderStarAnimation();
        yield return new WaitForSeconds(2f);
        scoreSlider.updateScore();
    }
    public IEnumerator DoScaleAnimation(Transform transform, Vector2 toScale, float waitTime, float time, Action EndAction = null)
    {
        
        float i = 0;
        Vector2 fromScale = transform.localScale;
        yield return new WaitForSeconds(waitTime);
        while (i < 1)
        {
            i += Time.deltaTime / time;
            transform.localScale = Vector2.LerpUnclamped(fromScale, toScale, i);
            yield return null;
        }
        EndAction?.Invoke();
    }

    public IEnumerator DoPositionAnimation(Transform transform, Vector2 toPosition, float waitTime, float time, Action EndAction = null)
    {
        float i = 0;
        Vector2 fromPosition = transform.localPosition;
        yield return new WaitForSeconds(waitTime);
        while (i < 1)
        {
            i += Time.deltaTime / time;
            transform.localPosition = Vector2.LerpUnclamped(fromPosition, toPosition, i);
            yield return null;
        }
        EndAction?.Invoke();
    }
    #endregion
}
