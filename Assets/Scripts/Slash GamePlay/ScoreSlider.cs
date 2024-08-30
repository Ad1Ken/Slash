using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSlider : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public Slider scoreSlider;
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    private void Start()
    {
        scoreSlider.value = 0;
        scoreSlider.maxValue = 5;
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Debug.Log("InUpdate ScoreSlider");
    //        updataScore();
    //    }
    //}
    #endregion

    #region PUBLIC_METHODS
    public void updataScore()
    {
        //scoreSlider.value += score;
        StartCoroutine(IncreaseSlider(scoreSlider.value, 2f));
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    private IEnumerator IncreaseSlider(float currentValue, float time )
    {
        float i = 0;
        currentValue = scoreSlider.value;
        float targetValue = currentValue + 1;
        
        while(i < 1)
        {
            i += Time.deltaTime / time;
            scoreSlider.value = Mathf.Lerp(currentValue, targetValue, i);
            yield return null;
        }
    }
    #endregion

}
