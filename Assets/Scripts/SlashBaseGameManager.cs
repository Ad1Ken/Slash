using UnityEngine;

public abstract class SlashBaseGameManager : MonoBehaviour
{
    public string activityNameDisplayText;
    public string activityDetailsDisplayText;
    public string activityHelpText;
    public string lastlevelPref;

    internal int currentLevel = 0;

    private int maxLevel = 15;

    public abstract void Init();
    public abstract void StartGame();
    public abstract void ResetActivity();
    public abstract void OnLevelComplete();
    public abstract void OnClickPause();
    public abstract void OnClickResume();

    internal virtual void LoadNextLevel()
    {
        Debug.Log("ChangeLevel");

        //ResetActivity();
        if (currentLevel >= maxLevel - 1)
        {
            currentLevel = 14;
            PlayerPrefs.SetInt(lastlevelPref, 15);
        }
        else if (currentLevel < maxLevel)
        {
            currentLevel += 1;
            PlayerPrefs.SetInt(lastlevelPref, currentLevel);
        }
        else
        {
            Debug.LogError("Should not come here");
        }
    }
    internal virtual void ChangeLevel()
    {
        ResetActivity();
    }
    public int GetcurrentLevel()
    {
        return currentLevel;
    }
    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt(lastlevelPref, 0);
    }
    internal void LoadCurrentLevel()
    {
        currentLevel = PlayerPrefs.GetInt(lastlevelPref, 0);
        if (currentLevel == 15)
        {
            currentLevel = 14;
        }
    }
}
