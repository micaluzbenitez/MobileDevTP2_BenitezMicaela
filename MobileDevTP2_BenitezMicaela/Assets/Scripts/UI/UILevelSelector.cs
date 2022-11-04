using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class UILevelSelector : MonoBehaviour
{
    [Header("Scenes")]
    public string gameSceneName = "";
    public string mainMenuSceneName = "";

    public void Level(int level)
    {
        PlayerPrefs.SetInt("Level", level);
        LoaderManager.Instance.LoadScene(gameSceneName);
    }

    public void MainMenu()
    {
        LoaderManager.Instance.LoadScene(mainMenuSceneName);
    }
}
