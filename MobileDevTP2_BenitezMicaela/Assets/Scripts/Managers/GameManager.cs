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
        [Header("Game data")]
        public GameData gameData = null;

        [Header("Spawner")]
        public Spawner spawner = null;

        [Header("Blade")]
        public Blade blade = null;

        [Header("UI")]
        public UIGame uiGame = null;

        private void Start()
        {
            blade.bladeTrail.colorGradient = gameData.bladeColor;
            NewGame();
        }

        private void OnEnable()
        {
            uiGame.OnRestartGame += NewGame;
        }

        private void OnDisable()
        {
            uiGame.OnRestartGame -= NewGame;
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