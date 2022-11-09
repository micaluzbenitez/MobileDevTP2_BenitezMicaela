using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UILevelSelector : MonoBehaviour
{
    [Header("Scenes")]
    public string gameSceneName = "";
    public string mainMenuSceneName = "";

    [Header("Levels")]
    public Image[] levelsImage = null;

    private void Awake()
    {
        for (int i = 0; i < levelsImage.Length; i++)
        {
            if (PlayerPrefs.GetInt($"Level{i + 1}") == 1) levelsImage[i].color = Color.white;
            else levelsImage[i].color = Color.black;
        }
    }

    public void Level(int level)
    {
        if (PlayerPrefs.GetInt($"Level{level}") == 1)
        {
            PlayerPrefs.SetInt("Level", level);
            LoaderManager.Instance.LoadScene(gameSceneName);
        }
    }

    public void MainMenu()
    {
        LoaderManager.Instance.LoadScene(mainMenuSceneName);
    }
}
