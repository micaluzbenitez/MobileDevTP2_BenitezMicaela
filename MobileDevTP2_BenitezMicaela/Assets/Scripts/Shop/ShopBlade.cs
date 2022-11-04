using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;

namespace Shop
{
    public class ShopBlade : MonoBehaviour
    {
        [Header("UI Shop")]
        public UIShop uiShop = null;

        [Header("Button")]
        public Text coinText = null;
        public Text unlocked = null;

        [Header("Blade")]
        public ColorBlade colorBlade = null;

        private void Awake()
        {
            if (PlayerPrefs.GetInt($"{colorBlade.ID}Unlocked") == 1)
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
            float totalCoins = PlayerPrefs.GetFloat("TotalCoins");

            if (PlayerPrefs.GetInt($"{colorBlade.ID}Unlocked") == 0)
            {
                if (totalCoins >= colorBlade.cost)
                {
                    totalCoins -= colorBlade.cost;
                    PlayerPrefs.SetFloat("TotalCoins", totalCoins);
                    PlayerPrefs.Save();

                    coinText.gameObject.SetActive(false);
                    unlocked.gameObject.SetActive(true);
                    uiShop.UpdateScore();

                    PlayerPrefs.SetInt($"{colorBlade.ID}Unlocked", 1);
                    PlayerPrefs.SetInt("BladeColor", colorBlade.ID);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                PlayerPrefs.SetInt("BladeColor", colorBlade.ID);
                PlayerPrefs.Save();
            }
        }
    }
}