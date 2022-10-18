using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ColorBlade : MonoBehaviour
    {
        [Header("Game data")]
        public GameData gameData = null;

        [Header("Button")]
        public Text coinText = null;
        public Text unlocked = null;

        [Header("Blade")]
        public Gradient color = null;
        public int cost = 0;
        public bool unlock = false;

        private void Awake()
        {
            coinText.text = "$" + cost;
        }

        public void ShopColorBlade()
        {
            if (!unlock)
            {
                if (gameData.totalCoins >= cost)
                {
                    gameData.totalCoins -= cost;
                    coinText.gameObject.SetActive(false);
                    unlocked.gameObject.SetActive(true);
                    unlock = true;
                }
            }

            if (unlock) gameData.bladeColor = color;
        }
    }
}