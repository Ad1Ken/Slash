using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlashSlider : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region  PUBLIC_PROPERTIES
    public Vector3 finalPosition; //final or end position after the drag
    #endregion

    #region PRIVATE_PROPERTIES
    [SerializeField] private RectTransform rectTransform;  // The RectTransform of the UI Image
    [SerializeField] private float leftBoundary = -200f;   // Left boundary
    [SerializeField] private float rightBoundary = 200f;   // Right boundary
    
    private Vector2 originalPosition;
    #endregion

    #region UNITY_CALLBACKS
    void Start()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        originalPosition = rectTransform.localPosition;
    }
    #endregion

    #region PUBLIC_METHODS
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Store the original position when dragging starts
        originalPosition = rectTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Calculate the new position
        Vector3 position = rectTransform.localPosition + new Vector3(eventData.delta.x, 0f, 0f);

        // Clamp the position to stay within the boundaries
        position.x = Mathf.Clamp(position.x, leftBoundary, rightBoundary);

        // Apply the new position
        rectTransform.localPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Optionally, you can add code here if you want to do something when the drag ends.
        finalPosition = rectTransform.localPosition;
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
