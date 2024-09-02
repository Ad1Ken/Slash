using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlashLoadingView : BaseView
{
    #region PUBLIC_PROPERTIES
    public Slider loadingslider;
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
        StartCoroutine(IncreaseSlider(1f,2f));
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

    private IEnumerator IncreaseSlider(float waitTime, float time)
    {
        yield return new WaitForSeconds(waitTime);
        float i = 0;

        while (i < 1)
        {
            i += Time.deltaTime / time;
            loadingslider.value = Mathf.Lerp(0f, 1f, i);
            yield return null;
        }
    }
    #endregion
}
