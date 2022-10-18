using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ColorBlade : MonoBehaviour
    {
        [Header("Button")]
        public Text coinText = null;

        [Header("Blade")]
        public Gradient color = null;
        public int cost = 0;
        public bool unlock = false;
        public bool equip = false;

        private void Awake()
        {
            coinText.text = "$" + cost;
        }
    }
}