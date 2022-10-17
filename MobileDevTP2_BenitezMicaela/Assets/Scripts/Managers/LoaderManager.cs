using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Toolbox;

public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}