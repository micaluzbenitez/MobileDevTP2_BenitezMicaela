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
        [Header("UI")]
        public TMP_Text scoreText = null;
        public Image fadeImage = null;
        public float fadeSpeed = 0f;

        [Header("Game over")]
        public Animator gameOverAnimator = null;

        [Header("Scenes")]
        public string shopSceneName = "";
        public string mainMenuSceneName = "";

        private int score = 0;
        private bool loser = false;
        private ColorLerper fadeLerper = new ColorLerper();

        public Action OnRestartGame = null;

        private void Awake()
        {
            gameOverAnimator.SetBool("Idle", true);
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
                    gameOverAnimator.SetBool("Idle", false);
                    gameOverAnimator.SetBool("Open", true);
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