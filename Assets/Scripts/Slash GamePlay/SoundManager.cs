using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public static SoundManager instance;
    #endregion

    #region PRIVATE_PROPERTIES
    [Header("AudioSources")]
    [SerializeField] private AudioSource audioSource;


    [Header("Common Sounds")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip onClickSound;
    [SerializeField] private AudioClip sliderFillUp;
    [SerializeField] private AudioClip popSound;
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region PUBLIC_METHODS   
    public void PlayCorrectSound()
    {
        audioSource.clip = correctSound;
        audioSource.Play();
    }
    public void PlayWrongSound()
    {
        audioSource.clip = wrongSound;
        audioSource.Play();
    }
    public void PlayClickSound()
    {
        audioSource.clip = onClickSound;
        audioSource.Play();
    }
    public void PlaySliderFillUpSound()
    {
        audioSource.clip = sliderFillUp;
        audioSource.Play();
    }
    public void PlayPopSound()
    {
        audioSource.clip = popSound;
        audioSource.Play();
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
