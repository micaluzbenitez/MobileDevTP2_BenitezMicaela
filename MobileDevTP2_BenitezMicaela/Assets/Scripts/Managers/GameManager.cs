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

        [Header("Spawner")]
        public Spawner spawner = null;

        [Header("Blade")]
        public Blade blade = null;
        public ColorBlade[] colors = null;

        [Header("UI")]
        public UIGame uiGame = null;

        private void Start()
        {
            level = PlayerPrefs.GetInt("Level");
            SetLevel();
            CheckBladeColor();
            NewGame();
        }

        private void Update()
        {
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
                    spawner.SetSpawner(gameData.levels[i].bombChance, gameData.levels[i].maxBombChance, gameData.levels[i].minSpawnDelay,
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
            uiGame.FadeToClear();
            spawner.enabled = true;
            blade.enabled = true;
            uiGame.RestartScore();
            ClearScene();
        }

        public void UpdateScore(int points)
        {
            uiGame.UpdateScore(points);
        }

        public void Explode()
        {
            spawner.enabled = false;
            blade.enabled = false;
            uiGame.GameOver();
            uiGame.FadeToBlack();
        }

        private void ClearScene()
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