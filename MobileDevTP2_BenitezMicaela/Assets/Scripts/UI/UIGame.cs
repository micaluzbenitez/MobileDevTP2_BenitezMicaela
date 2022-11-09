using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Toolbox.Lerpers;
using Managers;

namespace UI
{
    public class UIGame : MonoBehaviour, IObservable
    {
        [Header("UI")]
        public TMP_Text scoreText = null;
        public TMP_Text timerText = null;
        public Image[] lifes = null;
        public Sprite lifeComplete = null;
        public Sprite loseLife = null;
        public Image fadeImage = null;
        public float fadeSpeed = 0f;

        [Header("Tutorial")]
        public Animator tutorialAnimator = null;

        [Header("Options")]
        public Animator optionsAnimator = null;

        [Header("Finish game")]
        public Animator winGameAnimator = null;
        public Animator loseGameAnimator = null;
        public GameObject nextLevelButton = null;
        public GameObject levelSelectionButton = null;
        public TMP_Text coinsText = null;
        public float coinsTextSpeed = 0f;

        [Header("Scenes")]
        public string shopSceneName = "";
        public string mainMenuSceneName = "";
        public string levelSelectorSceneName = "";

        private int score = 0;
        private bool finishGame = false;
        private bool gameOver = false;
        private ColorLerper fadeLerper = new ColorLerper();
        private FloatLerper coinsLerper = new FloatLerper();

        private List<IObserver> observers = new List<IObserver>();

        private void Awake()
        {
            optionsAnimator.SetBool("Idle", true);
            winGameAnimator.SetBool("Idle", true);
            loseGameAnimator.SetBool("Idle", true);
            coinsLerper.unscaleTimer = true;
            Tutorial();
        }

        private void Update()
        {
            UpdateFade();
            TotalCoinsLerper();
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
                if (finishGame)
                {
                    SetFinishGamePanel();
                    GameManager.Instance.ClearScene();
                }
            }
        }

        private void SetFinishGamePanel()
        {
            if (!gameOver)
            {
                winGameAnimator.SetBool("Idle", false);
                winGameAnimator.SetBool("Open", true);

                if (PlayerPrefs.GetInt("Level") == 5)
                {
                    nextLevelButton.SetActive(false);
                    levelSelectionButton.SetActive(true);
                }
                else
                {
                    nextLevelButton.SetActive(true);
                    levelSelectionButton.SetActive(false);
                }

                CalculateTotalCoins();
            }
            else
            {
                loseGameAnimator.SetBool("Idle", false);
                loseGameAnimator.SetBool("Open", true);
            }

            Time.timeScale = 0;
            finishGame = false;
        }

        private void CalculateTotalCoins()
        {
            float totalCoins = PlayerPrefs.GetFloat("TotalCoins");
            coinsLerper.SetLerperValues(totalCoins, totalCoins + score, coinsTextSpeed, Lerper<float>.LERPER_TYPE.STEP_SMOOTH, true);
            coinsText.text = "$" + totalCoins;
            totalCoins += score;
            PlayerPrefs.SetFloat("TotalCoins", totalCoins);
            PlayerPrefs.Save();
        }

        private void TotalCoinsLerper()
        {
            if (coinsLerper.Active)
            {
                coinsLerper.UpdateLerper();
                coinsText.text = "$" + (int)coinsLerper.GetValue();
            }
        }

        public void RestartUI()
        {
            score = 0;
            scoreText.text = "Score: " + score;

            for (int i = 0; i < lifes.Length; i++)
            {
                lifes[i].sprite = lifeComplete;
            }
        }

        public void UpdateScore(int points)
        {
            score += points;
            scoreText.text = "Score: " + score;
        }

        public void UpdateTimer(float timer)
        {
            timer += 1;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void UpdateLifes(int actualLifes)
        {
            for (int i = 0; i < lifes.Length; i++)
            {
                if (!(i < actualLifes)) lifes[i].sprite = loseLife;
            }
        }

        public void FinishGame(bool gameOver)
        {
            finishGame = true;
            this.gameOver = gameOver;
        }

        public void FadeToBlack()
        {
            fadeLerper.SetLerperValues(Color.clear, Color.black, fadeSpeed, Lerper<Color>.LERPER_TYPE.STEP_SMOOTH, true);
        }

        public void FadeToClear()
        {
            fadeLerper.SetLerperValues(Color.black, Color.clear, fadeSpeed, Lerper<Color>.LERPER_TYPE.STEP_SMOOTH, true);
        }

        public void Tutorial()
        {
            tutorialAnimator.SetBool("Idle", false);
            tutorialAnimator.SetBool("Open", true);
            Time.timeScale = 0;
        }

        public void CloseTutorial()
        {
            tutorialAnimator.SetBool("Open", false);
            Time.timeScale = 1;
        }

        public void Options()
        {
            optionsAnimator.SetBool("Idle", false);
            optionsAnimator.SetBool("Open", true);
            Time.timeScale = 0;
        }

        public void CloseOptions()
        {
            optionsAnimator.SetBool("Open", false);
            Time.timeScale = 1;
        }

        public void NextLevel()
        {
            PlayAgain(winGameAnimator);
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            GameManager.Instance.SetLevel();
        }

        public void Replay()
        {
            PlayAgain(loseGameAnimator);
        }

        public void PlayAgain(Animator panel)
        {
            Notify();
            FadeToClear();
            RestartUI();
            panel.SetBool("Open", false);
            Time.timeScale = 1;
        }

        public void LevelSelector()
        {
            LoaderManager.Instance.LoadScene(levelSelectorSceneName);
            Time.timeScale = 1;
        }

        public void Shop()
        {
            LoaderManager.Instance.LoadScene(shopSceneName);
            Time.timeScale = 1;
        }

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
            Time.timeScale = 1;
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update(this);
            }
        }
    }
}