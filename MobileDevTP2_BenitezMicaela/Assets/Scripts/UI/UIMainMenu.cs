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
        public string logsSceneName = "";

        [Header("Options")]
        public Animator optionsAnimator = null;

        private void Awake()
        {
            optionsAnimator.SetBool("Idle", true);
        }

        public void Options()
        {
            optionsAnimator.SetBool("Idle", false);
            optionsAnimator.SetBool("Open", true);
            Time.timeScale = 0;
        }

        public void Logs()
        {
            LoaderManager.Instance.LoadScene(logsSceneName);
            Time.timeScale = 1;
        }

        public void CloseOptions()
        {
            optionsAnimator.SetBool("Open", false);
            Time.timeScale = 1;
        }

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