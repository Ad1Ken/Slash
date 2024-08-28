using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashUIManager : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public static SlashUIManager instance { get; private set; }

    public SlashMainView slashMainView;
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
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
