// Juliet Gobran

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressSlider;

    public void LoadingLevel (int sceneIndex)
    {
       StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            
            Debug.Log(progress);
            progressSlider.value = progress;

            yield return null;
        }
    }
}
