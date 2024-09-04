using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlashActivityItem : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public int activityIndex;
    public TextMeshProUGUI levelText;
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] private GameObject starHolder;
    [SerializeField] private List<Image> startImages;
    #endregion

    #region UNITY_CALLBACKS
    #endregion

    #region PUBLIC_METHODS
    public void OnClickPlay()
    {
        StartCoroutine(SlashManager.Instance.StartLoadingData(activityIndex));
        StartCoroutine(SlashUIManager.instance.panelActivityView.IncreaseSlider(0f,3f));
    }
    public void SetLevelText()
    {
        levelText.text = (activityIndex + 1).ToString();
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    //private IEnumerator StartLoadingData()
    //{
    //    SlashManager.Instance.Init(activityIndex);
    //    yield return new WaitForSeconds(3f);
    //    SlashUIManager.instance.panelActivityView.HideView();
    //}
    #endregion
}
