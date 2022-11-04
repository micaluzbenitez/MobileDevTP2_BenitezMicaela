using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace UI
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("Scenes")]
        public string levelSelectorSceneName = "";
        public string shopSceneName = "";
        public string creditsSceneName = "";

        public void Play()
        {
            LoaderManager.Instance.LoadScene(levelSelectorSceneName);
        }

        public void Shop()
        {
            LoaderManager.Instance.LoadScene(shopSceneName);
        }

        public void Credits()
        {
            LoaderManager.Instance.LoadScene(creditsSceneName);
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}