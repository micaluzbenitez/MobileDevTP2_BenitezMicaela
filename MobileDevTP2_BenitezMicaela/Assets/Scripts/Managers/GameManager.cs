using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public Image fadeImage = null;

        private int score = 0;

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            Time.timeScale = 1f;
            spawner.enabled = true;
            blade.enabled = true;

            score = 0;
            scoreText.text = "Score: " + score;
            ClearScene();
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

        public void UpdateScore(int points)
        {
            score += points;
            scoreText.text = "Score: " + score;
        }

        public void Explode()
        {
            spawner.enabled = false;
            blade.enabled = false;
            StartCoroutine(ExplodeSequence());
        }

        private IEnumerator ExplodeSequence()
        {
            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                float lerperTime = Mathf.Clamp01(elapsed / duration);
                fadeImage.color = Color.Lerp(Color.clear, Color.black, lerperTime);

                Time.timeScale = 1f - lerperTime;
                elapsed += Time.unscaledDeltaTime;

                yield return null;
            }

            yield return new WaitForSecondsRealtime(1f);

            NewGame();

            elapsed = 0;

            while (elapsed < duration)
            {
                float lerperTime = Mathf.Clamp01(elapsed / duration);
                fadeImage.color = Color.Lerp(Color.black, Color.clear, lerperTime);

                elapsed += Time.unscaledDeltaTime;

                yield return null;
            }
        }
    }
}