using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Toolbox;

namespace Managers
{
    public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
    {
        const string loadingScene = "LoadingScene";
        const float waitTimer = 1f;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(InternalLoadScene(sceneName));
        }

        private IEnumerator InternalLoadScene(string sceneName)
        {
            SceneManager.LoadScene(loadingScene);

            yield return null;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                // Se completo la carga
                if (asyncLoad.progress >= 0.9f)
                    asyncLoad.allowSceneActivation = true;

                yield return new WaitForSeconds(waitTimer);
            }

            yield return new WaitForSeconds(waitTimer);
            asyncLoad.allowSceneActivation = true;
        }
    }
}