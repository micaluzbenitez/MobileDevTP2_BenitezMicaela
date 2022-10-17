using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Toolbox;
using Toolbox.Pool;
using Toolbox.Lerpers;

namespace Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [Header("Spawner")]
        public Spawner spawner = null;

        [Header("Blade")]
        public Blade blade = null;

        [Header("UI")]
        public TMP_Text scoreText = null;
        public Image fadeImage = null;
        public float fadeSpeed = 0f;

        private int score = 0;
        private bool loser = false;
        private ColorLerper fadeLerper = new ColorLerper();

        private void Start()
        {
            NewGame();
        }

        private void Update()
        {
            UpdateFade();
        }

        private void NewGame()
        {
            FadeToClear();
            spawner.enabled = true;
            blade.enabled = true;
            loser = false;

            score = 0;
            scoreText.text = "Score: " + score;
            ClearScene();
        }

        private void ClearScene()
        {
            ObjectPooler pooler = ObjectPooler.Instance;
            pooler.TurnOffAllPoolObjects();
        }

        public void UpdateScore(int points)
        {
            score += points;
            scoreText.text = "Score: " + score;
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
                if (loser) NewGame();
            }
        }

        public void Explode()
        {
            spawner.enabled = false;
            blade.enabled = false;
            loser = true;
            FadeToBlack();
        }

        private void FadeToBlack()
        {
            fadeLerper.SetLerperValues(Color.clear, Color.black, fadeSpeed, Lerper<Color>.LERPER_TYPE.STEP_SMOOTH, true);
        }

        private void FadeToClear()
        {
            fadeLerper.SetLerperValues(Color.black, Color.clear, fadeSpeed, Lerper<Color>.LERPER_TYPE.STEP_SMOOTH, true);
        }
    }
}