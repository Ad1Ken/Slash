using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ImageHandler : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public SlashSlider slashSlider;

    public Image originalImage; // Assign your image in the inspector
    public Image leftImage;     // Assign the first part of the image
    public Image rightImage;    // Assign the second part of the image

    public Animator axeAnimator;
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    #endregion

    #region PUBLIC_METHODS
    public void SplitImage(float splitPercentage)
    {
        Sprite originalSprite = originalImage.sprite; //original Sprite

        //Calculate the width of the split based on the given percentage.
        float splitWidth = originalSprite.rect.width * (splitPercentage / 100f);

        //Create the first sprite that represents the left part (or first part) of the image.
        Rect firstRect = new Rect(originalSprite.rect.x, originalSprite.rect.y, splitWidth, originalSprite.rect.height);
        Sprite firstSprite = Sprite.Create(originalSprite.texture, firstRect, new Vector2(0.5f, 0.5f));


        //Create the second sprite that represents the right part (or second part) of the image.
        Rect secondRect = new Rect(originalSprite.rect.x + splitWidth, originalSprite.rect.y, originalSprite.rect.width - splitWidth, originalSprite.rect.height);
        Sprite secondSprite = Sprite.Create(originalSprite.texture, secondRect, new Vector2(0.5f, 0.5f));



        GameObject firstPart = new GameObject("FirstPart");
        SpriteRenderer firstRenderer = firstPart.AddComponent<SpriteRenderer>();
        //firstRenderer.sprite = firstSprite;
        leftImage.sprite = firstSprite;

        GameObject secondPart = new GameObject("SecondPart");
        SpriteRenderer secondRenderer = secondPart.AddComponent<SpriteRenderer>();
        //secondRenderer.sprite = secondSprite;
        rightImage.sprite = secondSprite;
        originalImage.gameObject.SetActive(false);
        leftImage.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);

        // Position the second part next to the first part
        secondPart.transform.position = new Vector3(firstPart.transform.position.x + (splitWidth / originalSprite.pixelsPerUnit), firstPart.transform.position.y,firstPart.transform.position.z);

    }



    public void SplitAndPositionImages(float percentage)
    {
        // Get original sprite and texture
        Sprite originalSprite = originalImage.sprite;
        Texture2D originalTexture = originalSprite.texture;

        // Calculate the width for each part
        int splitWidth = Mathf.RoundToInt(originalTexture.width * (percentage / 100f));
        int remainingWidth = originalTexture.width - splitWidth;

        // Create textures for both parts
        Texture2D leftTexture = new Texture2D(splitWidth, originalTexture.height);
        Texture2D rightTexture = new Texture2D(remainingWidth, originalTexture.height);

        // Copy pixels for the left part
        leftTexture.SetPixels(originalTexture.GetPixels(0, 0, splitWidth, originalTexture.height));
        leftTexture.Apply();

        // Copy pixels for the right part
        rightTexture.SetPixels(originalTexture.GetPixels(splitWidth, 0, remainingWidth, originalTexture.height));
        rightTexture.Apply();

        // Create sprites from the new textures
        Sprite leftSprite = Sprite.Create(leftTexture, new Rect(0, 0, splitWidth, originalTexture.height), new Vector2(0.5f, 0.5f));
        Sprite rightSprite = Sprite.Create(rightTexture, new Rect(0, 0, remainingWidth, originalTexture.height), new Vector2(0.5f, 0.5f));

        // Assign sprites to the left and right images
        leftImage.sprite = leftSprite;
        rightImage.sprite = rightSprite;

        // Adjust the RectTransform of left and right images
        RectTransform originalRectTransform = originalImage.rectTransform;
        RectTransform leftRectTransform = leftImage.rectTransform;
        RectTransform rightRectTransform = rightImage.rectTransform;

        // Match size and position for left image
        leftRectTransform.anchorMin = originalRectTransform.anchorMin;
        leftRectTransform.anchorMax = originalRectTransform.anchorMax;
        leftRectTransform.pivot = originalRectTransform.pivot;
        leftRectTransform.sizeDelta = new Vector2(originalRectTransform.sizeDelta.x * (percentage / 100f), originalRectTransform.sizeDelta.y);
        leftRectTransform.localPosition = originalRectTransform.localPosition + new Vector3(-(originalRectTransform.sizeDelta.x - leftRectTransform.sizeDelta.x) / 2, 0, 0);

        // Match size and position for right image
        rightRectTransform.anchorMin = originalRectTransform.anchorMin;
        rightRectTransform.anchorMax = originalRectTransform.anchorMax;
        rightRectTransform.pivot = originalRectTransform.pivot;
        rightRectTransform.sizeDelta = new Vector2(originalRectTransform.sizeDelta.x * ((100f - percentage) / 100f), originalRectTransform.sizeDelta.y);
        rightRectTransform.localPosition = originalRectTransform.localPosition + new Vector3((leftRectTransform.sizeDelta.x) / 2, 0, 0);
        originalImage.gameObject.SetActive(false);
        leftImage.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
