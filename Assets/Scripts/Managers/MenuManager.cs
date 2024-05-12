using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Animator creditsAnimator;
    [SerializeField] Animator settingsAnimator;
    [SerializeField] GameObject playPressPrefab;
    [SerializeField] GameObject clickPrefab;

    bool creditsDisplay = false;
    bool settingsDisplay = false;

    public void PressPlay()
    {
        Instantiate(playPressPrefab);
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(0.1f);
        StartCoroutine(LoadScene("testing"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        GameObject.Find("FadeCanvas").GetComponent<Animator>().SetBool("exit", true);
        yield return new WaitForSeconds(1.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void OpenSettings()
    {
        Instantiate(clickPrefab);
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(0.1f);
        settingsDisplay = !settingsDisplay;
        settingsAnimator.SetBool("display", settingsDisplay);
    }

    public void ToggleCredits()
    {
        Instantiate(clickPrefab);
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(0.1f);
        creditsDisplay = !creditsDisplay;
        creditsAnimator.SetBool("display", creditsDisplay);
    }

    public void PressBack()
    {
        Instantiate(clickPrefab);
        StatManager.instance.SaveData();
        StartCoroutine(LoadScene("menu"));
    }

    public void QuitGame()
    {
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        GameObject.Find("FadeCanvas").GetComponent<Animator>().SetBool("exit", true);
        yield return new WaitForSeconds(1.5f);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
