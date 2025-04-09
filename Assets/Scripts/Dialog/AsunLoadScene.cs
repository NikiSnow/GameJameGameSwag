using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.SceneManagement;
using System.Threading;

public class AsunLoadScene : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    private AsyncOperation asyncLoad;
    [SerializeField] float LoadingPersent;
    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync()
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

        while (!asyncLoad.isDone)
        {
            LoadingPersent = progress;
            // Обновить текст загрузки или полосу прогресса здесь
            yield return null;
        }
    }

    public void ChangeLoadActi()
    {
        asyncLoad.allowSceneActivation = true;
    }
}
