using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class UILevelSelector : MonoBehaviour
{
    [Header("Scenes")]
    public string mainMenuSceneName = "";

    public void MainMenu()
    {
        LoaderManager.Instance.LoadScene(mainMenuSceneName);
    }
}
