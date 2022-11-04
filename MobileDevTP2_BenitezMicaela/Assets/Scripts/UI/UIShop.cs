using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using TMPro;

namespace UI
{
    public class UIShop : MonoBehaviour
    {
        [Header("UI Shop")]
        public TMP_Text scoreText = null;

        [Header("Scenes")]
        public string levelSelectorSceneName = "";
        public string mainMenuSceneName = "";

        private void Awake()
        {
            UpdateScore();
        }

        public void Play()
        {
            LoaderManager.Instance.LoadScene(levelSelectorSceneName);
        }

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
        }

        public void UpdateScore()
        {
            scoreText.text = PlayerPrefs.GetFloat("TotalCoins").ToString();
        }
    }
}