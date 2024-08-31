using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlashUIManager : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public static SlashUIManager instance { get; private set; }

    public SlashMainView slashMainView;
    public SlashActivityView panelActivityView;
    public SlashWinPanel panelWinView;
    public SlashLosePanel panelLoseView; 
    public SlashLoadingView panelLoadingView;
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region PUBLIC_METHODS
    public void HideAllPanel()
    {
        slashMainView.HideView();
        panelActivityView.HideView();   
        panelWinView.HideView();
        panelLoseView.HideView();
        panelLoadingView.HideView();
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
