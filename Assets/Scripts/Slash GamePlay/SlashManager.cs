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
    private int currentRound = 0;
    private int totalScore;
    private float errorMarginPercentage = 10;
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        //Init(currentLevel);
    }
    #endregion

    #region PUBLIC_METHODS
    public override void Init(int levelIndex)
    {
        currentLevel = levelIndex;
        StartCoroutine(StartGame());
    }

    public override void ResetActivity()
    {
        ResetRound();
        totalScore = 0;
        currentRound = 0;
        SlashUIManager.instance.slashMainView.scoreSlider.ResetSlider();
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
    public void CalculatePercentaggeByUser(float finalPosition) //Calculate the percentage by user
    {
        //float finalPostion = SlashUIManager.instance.slashMainView.imageHandler.slashSlider.finalPosition.x;//final position at which user stopped the slider
        Debug.Log("finalPostion: " + finalPosition);
        float positionRespectToWidth = (boxWidth/2) + finalPosition;// calculated position according to the width of the box or item
        Debug.Log("positionRespectToWidth: " + positionRespectToWidth);
        percentageByUser = (positionRespectToWidth / boxWidth) * 100;//calculated percentage
        Debug.Log("percentageByUser: " + percentageByUser);

        SlashUIManager.instance.slashMainView.SplitImageRoutine(percentageByUser);
    }

    public bool isValidPosition(float finalPosition) // To Check that user percentage is correct or wrong
    {
        CalculatePercentaggeByUser(finalPosition);
        if (percentageByUser == percentageToSlash || (percentageByUser >= percentageToSlash - errorMarginPercentage 
            && percentageByUser <= percentageToSlash + errorMarginPercentage))
            return true;
        return false;
    }

    public void isAnswerSubmitted(float finalPosition) //Called after the used done with adjusting the slider
    {
        if (isValidPosition(finalPosition))
            StartCoroutine(OnWin());
        else
            StartCoroutine(OnLose());

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
        totalRounds = 5;
    }

    private void SetRoundParameters()
    {
        SlashUIManager.instance.slashMainView.SetImageToSlash(levelData.perLevelData[0].levelParameters[currentRound].itemImage.sprite);
        percentageToSlash = levelData.perLevelData[0].levelParameters[currentRound].percentage;
        SlashUIManager.instance.slashMainView.SetPercentageText(percentageToSlash);
    }
    private void  ResetRound()
    {
        SlashUIManager.instance.slashMainView.imageHandler.ResetImages();
    }
    
#endregion

#region DELEGTE_CALLBACK
#endregion

#region Coroutines
    private IEnumerator OnWin() // Called when round wins
    {
        SoundManager.instance.PlayCorrectSound();
        totalScore++;
        StartCoroutine(SlashUIManager.instance.slashMainView.UpdateScoreAnim());
        yield return new WaitForSeconds(5f);
        Generate();
    }
    private IEnumerator OnLose() // Called when round Lose
    {
        SoundManager.instance.PlayWrongSound();
        yield return new WaitForSeconds(5f);
        Generate();
    }
    private IEnumerator StartGame()
    {
        SlashUIManager.instance.slashMainView.ShowView();
        ResetActivity();
        SetLevelParameters();
        yield return new WaitForSeconds(3f);
        Generate();
    }

    public IEnumerator StartLoadingData(int activityIndex) // Load LevelData for according to activity
    {
        Init(activityIndex);
        yield return new WaitForSeconds(3f);
        SlashUIManager.instance.panelActivityView.HideView();
    }
    #endregion
}

