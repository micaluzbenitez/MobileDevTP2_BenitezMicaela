using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopBlade : MonoBehaviour
    {
        [Header("Game data")]
        public GameData gameData = null;

        [Header("Button")]
        public Text coinText = null;
        public Text unlocked = null;

        [Header("Blade")]
        public ColorBlade colorBlade = null;

        private void Awake()
        {
            if (colorBlade.unlock)
            {
                coinText.gameObject.SetActive(false);
                unlocked.gameObject.SetActive(true);
            }
            else
            {
                coinText.text = "$" + colorBlade.cost;
            }
        }

        public void ShopColorBlade()
        {
            if (!colorBlade.unlock)
            {
                if (gameData.totalCoins >= colorBlade.cost)
                {
                    gameData.totalCoins -= colorBlade.cost;
                    coinText.gameObject.SetActive(false);
                    unlocked.gameObject.SetActive(true);
                    colorBlade.unlock = true;
                }
            }

            if (colorBlade.unlock) gameData.bladeColor = colorBlade.color;
        }
    }
}