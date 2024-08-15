using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : SlashBaseGameManager
{
    #region PUBLIC_PROPERTIES
    public static SlashManager Instance { get; private set; }


    public float percentageToSlash;
    #endregion

    #region PRIVATE_PROPERTIES

    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region PUBLIC_METHODS
    public override void Init()
    {

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
#endregion

#region PRIVATE_METHODS

    private float GetRandomPercentage()
    {
        return Random.Range(1, 100);
    }
#endregion

#region DELEGTE_CALLBACK
#endregion

#region Coroutines
#endregion
}

