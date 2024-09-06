using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlashActivityView : BaseView
{
    #region PUBLIC_PROPERTIES
    public SlashActivityItem activityItemPrefab;
    public GameObject content;
    public Slider loadingSlider;
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    private void Start()
    {
        StartCoroutine(InstantiateActivities());
    }
    #endregion

    #region PUBLIC_METHODS
    public void OnClickExit()
    {
        SoundManager.instance.PlayClickSound();
        Application.Quit();
    }
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
            SoundManager.instance.PlayPopSound();
            //item.transform.localScale = Vector2.zero;
            //item.GetComponent<RectTransform>().DOScale(1f, 0.2f);
            //Image temp = item.GetComponent<Image>();
            //temp.sprite = activityFeatureImages[i];
            item.activityIndex = i;
            item.SetLevelText();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator IncreaseSlider(float waitTime, float time)
    {
        yield return new WaitForSeconds(waitTime);
        float i = 0;

        while (i < 1)
        {
            i += Time.deltaTime / time;
            loadingSlider.value = Mathf.Lerp(0f, 1f, i);
            yield return null;
        }
    }
    #endregion
}
