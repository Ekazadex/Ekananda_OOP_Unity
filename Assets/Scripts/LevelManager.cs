using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton instance
    public static LevelManager Instance { get; private set; }

    [SerializeField] private Animator animator;

    private void Awake()
    {
        // Ensure only one instance of LevelManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make it persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }
    
    // Coroutine to load the scene asynchronously with animation
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            yield break; // Exit if the scene is already loaded
        }

        // Play start transition animation if animator is assigned
        if (animator != null)
        {
            animator.SetTrigger("StartTransition");
            Debug.Log("Transition animation triggered");
        }

        // Wait for the transition to finish (adjust timing as needed)
        yield return new WaitForSeconds(1f);

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Play end transition animation if animator is assigned
        if (animator != null)
        {
            animator.SetTrigger("EndTransition");
        }
    }

    // Public method to start scene loading with transition
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Method to clear unnecessary objects in the scene
    public void ClearScene()
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.tag != "MainCamera" && obj.tag != "Player")
            {
                Destroy(obj);
            }
        }
    }
}
