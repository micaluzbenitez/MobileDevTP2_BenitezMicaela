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

        [Header("Game data")]
        public GameData gameData = null;

        [Header("Scenes")]
        public string gameSceneName = "";
        public string mainMenuSceneName = "";

        private void Awake()
        {
            UpdateScore();
        }

        public void Play()
        {
            LoaderManager.Instance.LoadScene(gameSceneName);
        }

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
        }

        public void UpdateScore()
        {
            scoreText.text = gameData.totalCoins.ToString();
        }
    }
}