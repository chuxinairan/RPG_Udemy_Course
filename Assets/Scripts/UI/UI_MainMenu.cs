using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    private void Start()
    {
        if (!SaveManager.instance.HasSavedData())
            continueButton.SetActive(false);
    }

    public void Continue()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.1f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSavedFile();
        StartCoroutine(LoadSceneWithFadeEffect(1.1f));
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private IEnumerator LoadSceneWithFadeEffect(float _second)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_second);
        SceneManager.LoadScene(sceneName);
    }
}
