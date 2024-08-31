using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashLoadingView : BaseView
{
    #region PUBLIC_PROPERTIES
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    private void Start()
    {
        StartCoroutine(StartInitialization(() =>
        {
            LoadMainMenu();
        }));
    }
    #endregion

    #region PUBLIC_METHODS
    #endregion

    #region PRIVATE_METHODS
    private void LoadMainMenu()
    {
        HideView();
        SlashUIManager.instance.panelActivityView.ShowView();
    }

    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    IEnumerator StartInitialization(Action callback = null)
    {
        FirebaseManager.Instance.InitializeFirebase();
        yield return new WaitForSeconds(1f);
        StartCoroutine(FirebaseManager.Instance.DatabaseServices.LoadData("LevelData"));
        yield return new WaitForSeconds(2f);
        callback?.Invoke();
    }
    #endregion
}
