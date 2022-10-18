using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Toolbox.Lerpers;
using Managers;

namespace UI
{
    public class UIGame : MonoBehaviour
    {
        [Header("Game data")]
        public GameData gameData = null;

        [Header("UI")]
        public TMP_Text scoreText = null;
        public Image fadeImage = null;
        public float fadeSpeed = 0f;

        [Header("Game over")]
        public Animator gameOverAnimator = null;
        public TMP_Text coinsText = null;
        public float coinsTextSpeed = 0f;

        [Header("Scenes")]
        public string shopSceneName = "";
        public string mainMenuSceneName = "";

        private int score = 0;
        private bool gameOver = false;
        private ColorLerper fadeLerper = new ColorLerper();
        private FloatLerper coinsLerper = new FloatLerper();

        public Action OnRestartGame = null;

        private void Awake()
        {
            gameOverAnimator.SetBool("Idle", true);
            coinsLerper.unscaleTimer = true;
        }

        private void Update()
        {
            UpdateFade();
            CalculateTotalCoins();
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
                if (gameOver)
                {
                    coinsLerper.SetLerperValues(gameData.totalCoins, gameData.totalCoins + score, coinsTextSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
                    coinsText.text = "$" + gameData.totalCoins;
                    gameData.totalCoins += score;

                    gameOverAnimator.SetBool("Idle", false);
                    gameOverAnimator.SetBool("Open", true);
                    Time.timeScale = 0;
                    gameOver = false;
                }
            }
        }

        private void CalculateTotalCoins()
        {
            if (coinsLerper.Active)
            {
                coinsLerper.UpdateLerper();
                coinsText.text = "$" + (int)coinsLerper.GetValue();
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

        public void GameOver()
        {
            gameOver = true;
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
            gameOverAnimator.SetBool("Open", false);
            Time.timeScale = 1;
        }

        public void Shop()
        {
            LoaderManager.Instance.LoadScene(shopSceneName);
            gameOverAnimator.SetBool("Open", false);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
            gameOverAnimator.SetBool("Open", false);
            Time.timeScale = 1;
        }
    }
}