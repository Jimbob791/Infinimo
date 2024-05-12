using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    [SerializeField] GameObject logoSFX;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(logoSFX);
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(LoadScene("menu"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
