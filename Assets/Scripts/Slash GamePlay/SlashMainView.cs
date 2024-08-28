using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlashMainView : MonoBehaviour
{
    #region  PUBLIC_PROPERTIES
    public ImageHandler imageHandler;
    public Coroutine splitCoroutine;

    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS

    #endregion

    #region PUBLIC_METHODS
    public void SplitImageRoutine(float percentage)
    {
        if (splitCoroutine != null) StopCoroutine(splitCoroutine); 
            splitCoroutine = StartCoroutine(SplitImage(percentage));
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    public IEnumerator SplitImage(float percentage)
    {
        //imageHandler.originalImage.gameObject.SetActive(false);  
        imageHandler.axeAnimator.Play("Axe"); 
        yield return new WaitForSeconds(1f);
        imageHandler.SplitAndPositionImages(percentage);
        //imageHandler.SplitImage(percentage);
    }
    #endregion
}
