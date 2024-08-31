using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashActivityView : BaseView
{
    #region PUBLIC_PROPERTIES
    public SlashActivityItem activityItemPrefab;
    public GameObject content;
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    #endregion

    #region PUBLIC_METHODS
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    private IEnumerator InstantiateActivities()
    {
        int activityCount = 5;
        for (int i = 0; i < activityCount; i++)
        {
            SlashActivityItem item = Instantiate(activityItemPrefab, content.transform);
            //item.transform.localScale = Vector2.zero;
            //item.GetComponent<RectTransform>().DOScale(1f, 0.2f);
            //Image temp = item.GetComponent<Image>();
            //temp.sprite = activityFeatureImages[i];
            item.activityIndex = i;
            item.SetLevelText();
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion
}
