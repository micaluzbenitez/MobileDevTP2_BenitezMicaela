using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Toolbox.Lerpers;

namespace UI
{
    public class UIGame : MonoBehaviour
    {
        [Header("UI")]
        public TMP_Text scoreText = null;
        public Image fadeImage = null;
        public float fadeSpeed = 0f;

        [Header("Game over")]
        public GameObject gameOver = null;

        [Header("Scenes")]
        public string shopSceneName = "";
        public string mainMenuSceneName = "";

        private int score = 0;
        private bool loser = false;
        private ColorLerper fadeLerper = new ColorLerper();

        public Action OnRestartGame = null;

        private void Awake()
        {
            gameOver.SetActive(false);
        }

        private void Update()
        {
            UpdateFade();
        }

        private void UpdateFade()
        {
            if (fadeLerper.Active)
            {
                fadeLerper.UpdateLerper();
                fadeImage.color = fadeLerper.GetValue();
            }

            if (fadeLerper.Reached)
            {
                if (loser)
                {
                    gameOver.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }

        public void RestartScore()
        {
            score = 0;
            scoreText.text = "Score: " + score;
        }

        public void UpdateScore(int points)
        {
            score += points;
            scoreText.text = "Score: " + score;
        }

        public void LoserState(bool state)
        {
            loser = state;
        }

        public void FadeToBlack()
        {
            fadeLerper.SetLerperValues(Color.clear, Color.black, fadeSpeed, Lerper<Color>.LERPER_TYPE.STEP_SMOOTH, true);
        }

        public void FadeToClear()
        {
            fadeLerper.SetLerperValues(Color.black, Color.clear, fadeSpeed, Lerper<Color>.LERPER_TYPE.STEP_SMOOTH, true);
        }

        public void Replay()
        {
            OnRestartGame?.Invoke();
            gameOver.SetActive(false);
            Time.timeScale = 1;
        }

        public void Shop()
        {
            LoaderManager.Instance.LoadScene(shopSceneName);
            gameOver.SetActive(false);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
            gameOver.SetActive(false);
            Time.timeScale = 1;
        }
    }
}