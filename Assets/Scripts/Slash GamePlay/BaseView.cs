using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : MonoBehaviour
{
 
    #region PUBLIC_PROPERTIES
    #endregion

    #region PUBLIC_METHODS
    public virtual void ShowView()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideView()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
