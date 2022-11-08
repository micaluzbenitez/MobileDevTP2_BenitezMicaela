using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Toolbox;
using UI;

namespace Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public int level = 0;

        [Header("Game data")]
        public GameData gameData = null;
        public float gameDuration = 0;
        public int totalLifes = 0;

        [Header("Spawner")]
        public Spawner spawner = null;

        [Header("Blade")]
        public Blade blade = null;
        public ColorBlade[] colors = null;

        [Header("UI")]
        public UIGame uiGame = null;

        private int lifes = 0;
        private bool finishGame = false;
        private Timer gameTimer = new Timer();

        private void Start()
        {
            gameTimer.SetTimer(gameDuration, Timer.TIMER_MODE.DECREASE, true);
            level = PlayerPrefs.GetInt("Level");
            SetLevel();
            CheckBladeColor();
            NewGame();
        }

        private void Update()
        {
            UpdateGameTimer();

            if (Input.GetKeyDown(KeyCode.M)) PlayerPrefs.DeleteAll();            
        }

        private void OnEnable()
        {
            uiGame.OnRestartGame += NewGame;
        }

        private void OnDisable()
        {
            uiGame.OnRestartGame -= NewGame;
        }

        private void SetLevel()
        {
            for (int i = 0; i < gameData.levels.Count; i++)
            {
                if (gameData.levels[i].level == level)
                {
                    spawner.SetSpawner(gameData.levels[i].bombChance, gameData.levels[i].maxSpawnDelay, gameData.levels[i].minSpawnDelay,
                                       gameData.levels[i].increaseDifficult, gameData.levels[i].timePerChange, gameData.levels[i].maxBombChance,
                                       gameData.levels[i].increaseBombChanceValue);
                }
            }
        }

        private void CheckBladeColor()
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].ID == PlayerPrefs.GetInt("BladeColor"))
                {
                    blade.bladeTrail.colorGradient = colors[i].color;
                }
            }
        }

        private void NewGame()
        {
            finishGame = false;
            lifes = totalLifes;
            uiGame.FadeToClear();
            spawner.enabled = true;
            blade.enabled = true;
            uiGame.RestartUI();
        }

        private void UpdateGameTimer()
        {
            if (gameTimer.Active)
            {
                gameTimer.UpdateTimer();
                uiGame.UpdateTimer(gameTimer.CurrentTime);
            }

            if (gameTimer.ReachedTimer())
            {
                Win();
                uiGame.UpdateTimer(-1);
            }
        }

        private void FinishGame(bool gameOver)
        {
            if (!finishGame)
            {
                spawner.enabled = false;
                blade.enabled = false;
                uiGame.FinishGame(gameOver);
                uiGame.FadeToBlack();
                finishGame = true;
            }
        }

        public void UpdateScore(int points)
        {
            uiGame.UpdateScore(points);
        }

        public void LoseLife()
        {
            if (!finishGame)
            {
                lifes--;
                uiGame.UpdateLifes(lifes);
                if (lifes <= 0) Lose();
            }
        }

        private void Win()
        {
            FinishGame(false);
        }

        public void Lose()
        {
            uiGame.UpdateLifes(0);
            FinishGame(true);
        }

        public void ClearScene()
        {
            Fruit[] fruits = FindObjectsOfType<Fruit>();
            foreach (Fruit fruit in fruits)
            {
                Destroy(fruit.gameObject);
            }

            Bomb[] bombs = FindObjectsOfType<Bomb>();
            foreach (Bomb bomb in bombs)
            {
                Destroy(bomb.gameObject);
            }
        }
    }
}