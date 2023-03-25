using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    private const string startGame = "HomeScreen";
    private void OnEnable()
    {
        StartLoadScene(startGame);
    }
    public void StartLoadScene(string sceneToLoad)
    {
        StartCoroutine(Loading(sceneToLoad));
    }

    // Coroutine for the loading effect. It use an alpha in out effect
    private IEnumerator Loading(string sceneToLoad)
    {
        LoadingFadeEffect.Instance.FadeIn();

        // Here the player still sees the black screen
        yield return new WaitUntil(() => LoadingFadeEffect.s_canLoad);

        LoadScene(sceneToLoad);

        yield return new WaitForSeconds(1f);
        LoadingFadeEffect.Instance.FadeOut();
    }

    private void LoadScene(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
