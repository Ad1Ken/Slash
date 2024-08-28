using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : SlashBaseGameManager
{
    #region PUBLIC_PROPERTIES
    public static SlashManager Instance { get; private set; }


    public float percentageByUser {  get; private set; }
    const int boxWidth =  900;


    public float percentageToSlash;
    #endregion

    #region PRIVATE_PROPERTIES

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
    public override void Init()
    {
        //CalculatePercentaggeByUser();
    }

    public override void OnClickPause()
    {

    }

    public override void OnClickResume()
    {

    }

    public override void OnLevelComplete()
    {

    }

    public override void ResetActivity()
    {

    }

    public override void StartGame()
    {

    }
    public void CalculatePercentaggeByUser()
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
        if(percentageByUser == percentageToSlash)
            return true;
        return false;
    }
    
    public void PlaySplitAnimation() 
    {
        SlashUIManager.instance.slashMainView.SplitImage(percentageByUser);
    }
#endregion  

#region PRIVATE_METHODS

    // Get random percentage for people to stop the Slider on
    private float GetRandomPercentage()
    {
        percentageToSlash = Random.Range(1, 100);
        return percentageToSlash;
    }
#endregion

#region DELEGTE_CALLBACK
#endregion

#region Coroutines
#endregion
}

