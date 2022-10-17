using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Spawner")]
        public Spawner spawner = null;

        [Header("Blade")]
        public Blade blade = null;

        [Header("UI")]
        public UIGame uiGame = null;

        public static GameManager Instance;
        public static GameManager Get() { return Instance; }

        private void Start()
        {
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
            uiGame.LoserState(false);
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
            uiGame.LoserState(true);
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