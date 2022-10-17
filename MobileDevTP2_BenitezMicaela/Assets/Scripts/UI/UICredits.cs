using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace UI
{
    public class UICredits : MonoBehaviour
    {
        [Header("Scenes")]
        public string mainMenuSceneName = "";

        public void MainMenu()
        {
            LoaderManager.Instance.LoadScene(mainMenuSceneName);
        }
    }
}