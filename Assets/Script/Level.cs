using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    int currentSceneIndex;
    [SerializeField] int timeToWait = 30;
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            StartCoroutine(WaitForTime());
        }
    }
    IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(timeToWait);
        LoadNextScene();
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Start Screen");
    }

    public void ReloadLevel()
    {
        
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; // loop back to start
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
