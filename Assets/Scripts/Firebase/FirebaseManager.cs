using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using Firebase;
using System;

public class FirebaseManager : MonoBehaviour
{
    #region PUBLIC_PROPERTIES
    public static FirebaseManager Instance;
    public DatabaseReference DatabaseReference;

    public DatabaseServices DatabaseServices;
    #endregion

    #region PRIVATE_PROPERTIES
    #endregion

    #region UNITY_CALLBACKS
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Exception);
            }
        });
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion

    #region DELEGTE_CALLBACKS
    #endregion

    #region Coroutines
    #endregion
}
