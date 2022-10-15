using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Toolbox;

namespace Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Spawner spawner = null;
        public Blade blade = null;

        [Header("UI")]
        public TMP_Text scoreText = null;

        private int score = 0;

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            score = 0;
            scoreText.text = "Score: " + score;
        }

        public void UpdateScore(int points)
        {
            score += points;
            scoreText.text = "Score: " + score;
        }

        public void Explode()
        {
            spawner.enabled = false;
            blade.enabled = false;
        }
    }
}