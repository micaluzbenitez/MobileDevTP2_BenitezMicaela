using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace UI
{
    public class UIShop : MonoBehaviour
    {
        [Header("Scenes")]
        public string gameSceneName = "";
        public string mainMenuSceneName = "";

        public void Play()
        {
            LoaderManager.Instance.LoadScene(gameSceneName);
        }

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
        }
    }
}