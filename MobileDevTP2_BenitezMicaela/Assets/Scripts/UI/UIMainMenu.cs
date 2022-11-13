using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

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
            if (PlayerPrefs.GetInt("Level1") == 0) PlayerPrefs.SetInt("Level1", 1);

            /// Achievement
            PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_first_achievement, 100.0f, success => { });
        }

        public void Options()
        {
            optionsAnimator.SetBool("Idle", false);
            optionsAnimator.SetBool("Open", true);
        }

        public void Logs()
        {
            LoaderManager.Instance.LoadScene(logsSceneName);
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Level1", 1);
            PlayerPrefs.Save();
        }

        public void CloseOptions()
        {
            optionsAnimator.SetBool("Open", false);
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