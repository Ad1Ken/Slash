using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashLosePanel : BaseView
{
    #region PUBLIC_PROPERTIES
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    #endregion

    #region PUBLIC_METHODS
    public void OnClicRetry()
    {
        StartCoroutine(SlashManager.Instance.StartLoadingData(SlashManager.Instance.currentLevel));
        HideView();
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
