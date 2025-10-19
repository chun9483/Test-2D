using UnityEngine;
using UnityEngine.SceneManagement; // Important: Include this namespace

public class SceneChanger : MonoBehaviour
{
    // Method to load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Method to load a scene by its build index
    public void LoadSceneByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}