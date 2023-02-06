using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLoad : MonoBehaviour
{
    public static MenuLoad instance;

    [Header("----- Loading Screen-----")]
    public GameObject loadingPanel;
    public Image loadingImage;


    float timer = 2;
    private void Awake()
    {
        instance = this;
    }

    public void LoadingScene(string sceneName)
    {
        StartCoroutine(WaitForFluff(timer, sceneName));
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
       
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            loadingImage.fillAmount = progress;

            yield return null;
        }


    }

    IEnumerator WaitForFluff(float num, string sceneName) 
    {
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(num);
        StartCoroutine(LoadAsyncScene(sceneName));
    }
}
