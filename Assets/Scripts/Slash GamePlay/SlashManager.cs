using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlashManager : SlashBaseGameManager
{
    #region PUBLIC_PROPERTIES
    public static SlashManager Instance { get; private set; }


    public float percentageByUser {  get; private set; }
    const int boxWidth =  900;


    public float percentageToSlash;

    public JSONLevelData jsonLevelData;
    public LevelData levelData;
    #endregion

    #region PRIVATE_PROPERTIES
    private int totalRounds;
    private int currentRound;
    private int totalScore;
    private float errorMarginPercentage = 3;
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        Init();
    }
    #endregion

    #region PUBLIC_METHODS
    public override void Init() //TODO
    {
        //CalculatePercentaggeByUser();
    }
    public override void StartGame() //TODO
    {
        ResetActivity();
        SetLevelParameters();
        Generate();
    }
    public override void ResetActivity()
    {
        ResetRound();
        totalScore = 0;
        currentRound = 0;
    }

    

    public override void OnLevelComplete()
    {
        if (currentRound == totalRounds)
        {
            if (totalScore >= 3)
            {
                if (currentLevel > GetTotalNumberOfActivities() - 1)
                    currentLevel = 0;
                SlashUIManager.instance.panelWinView.ShowView();
                currentLevel++;
            }
            else
            {
                SlashUIManager.instance.panelLoseView.ShowView();
            }
        }
        else
        {
            Debug.Log("Rounds Not Completed");
        }
    }

    public void Generate()
    {
        if(currentRound < totalRounds)
        {
            ResetRound();
            SetRoundParameters();
            currentRound++;
        }
        else
        {
            OnLevelComplete();
        }
    }
    public void CalculatePercentaggeByUser() // To do Calling properly
    {
        float finalPostion = SlashUIManager.instance.slashMainView.imageHandler.slashSlider.finalPosition.x;//final position at which user stopped the slider
        Debug.Log("finalPostion: " + finalPostion);
        float positionRespectToWidth = (boxWidth/2) + finalPostion;// calculated position according to the width of the box or item
        Debug.Log("positionRespectToWidth: " + positionRespectToWidth);
        percentageByUser = (positionRespectToWidth / boxWidth) * 100;//calculated percentage
        Debug.Log("percentageByUser: " + percentageByUser);

        SlashUIManager.instance.slashMainView.SplitImageRoutine(percentageByUser);
    }

    public bool isValidPosition()
    {
        if(percentageByUser == percentageToSlash || (percentageByUser >= percentageToSlash - errorMarginPercentage 
            && percentageByUser <= percentageToSlash + errorMarginPercentage))
            return true;
        return false;
    }
    
    public void PlaySplitAnimation() 
    {
        SlashUIManager.instance.slashMainView.SplitImage(percentageByUser);
    }
#endregion  

#region PRIVATE_METHODS
    private int GetTotalNumberOfActivities()
    {
        return levelData.perLevelData.Length;
    }
    private void SetLevelParameters()
    {
        SlashUIManager.instance.slashMainView.StartDownloadImageRoutine(currentLevel);
    }

    private void SetRoundParameters()
    {
        SlashUIManager.instance.slashMainView.SetImageToSlash(levelData.perLevelData[currentLevel].levelParameters[currentRound].itemImage);
        percentageToSlash = levelData.perLevelData[currentLevel].levelParameters[currentRound].percentage;
    }
    private void  ResetRound()
    {
        SlashUIManager.instance.slashMainView.imageHandler.ResetImages();
    }
    // Get random percentage for people to stop the Slider on
    //private float GetRandomPercentage()
    //{
    //    percentageToSlash = Random.Range(1, 100);
    //    return percentageToSlash;
    //}
#endregion

#region DELEGTE_CALLBACK
#endregion

#region Coroutines
    private IEnumerator OnWin() //TODO
    {
        totalScore++;
        Generate();
        yield return null;
    }
    private IEnumerator OnLose() //TODO
    {
        yield return null;
        Generate();
    }
#endregion
}

