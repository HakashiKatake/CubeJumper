using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages clean scene transitions with proper cleanup
/// Ensures no audio or objects persist between scenes
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    
    [Header("Transition Settings")]
    [Tooltip("Fade screen to black during transition")]
    public bool useFadeTransition = false;
    
    [Tooltip("Duration of fade effect")]
    public float fadeDuration = 0.5f;
    
    private bool isTransitioning = false;
    
    void Awake()
    {
        // Don't persist across scenes - each scene gets fresh manager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Loads a scene with complete cleanup
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("Scene transition already in progress!");
            return;
        }
        
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    /// <summary>
    /// Coroutine that handles clean scene loading
    /// </summary>
    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isTransitioning = true;
        
        Debug.Log($"SceneTransitionManager: Starting transition to {sceneName}");
        
        // 1. Stop all audio
        StopAllAudio();
        
        // 2. Reset time scale (in case game was paused)
        Time.timeScale = 1f;
        
        // 3. Optional: Fade out effect
        if (useFadeTransition)
        {
            // You can add fade-to-black effect here if you have a UI panel
            yield return new WaitForSeconds(fadeDuration);
        }
        
        // 4. Clean up any persistent objects that shouldn't persist
        CleanupPersistentObjects();
        
        // Small delay to ensure cleanup completes
        yield return null;
        
        // 5. Load the new scene
        Debug.Log($"SceneTransitionManager: Loading scene {sceneName}");
        SceneManager.LoadScene(sceneName);
        
        isTransitioning = false;
    }
    
    /// <summary>
    /// Stops all audio sources in the current scene
    /// </summary>
    void StopAllAudio()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>(true);
        
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.clip = null; // Clear the clip to free memory
                Debug.Log($"Stopped and cleared audio from: {audioSource.gameObject.name}");
            }
        }
        
        // Also stop any audio listeners
        AudioListener.pause = false;
    }
    
    /// <summary>
    /// Cleans up objects that might persist incorrectly
    /// </summary>
    void CleanupPersistentObjects()
    {
        // Find objects marked with DontDestroyOnLoad (except HighScoreManager)
        GameObject[] rootObjects = gameObject.scene.GetRootGameObjects();
        
        foreach (GameObject obj in rootObjects)
        {
            // Skip HighScoreManager and EventSystem (should persist)
            if (obj.name.Contains("HighScore") || obj.name.Contains("EventSystem"))
            {
                continue;
            }
            
            // Check if object is in DontDestroyOnLoad scene
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                // Destroy old AudioManagers, GameModeManagers, etc.
                if (obj.GetComponent<AudioManager>() != null ||
                    obj.GetComponent<GameModeManager>() != null ||
                    obj.GetComponent<MusicAnalyzer>() != null)
                {
                    Debug.Log($"Cleaning up persistent object: {obj.name}");
                    Destroy(obj);
                }
            }
        }
    }
    
    /// <summary>
    /// Quick scene load without coroutine (for immediate transitions)
    /// </summary>
    public void LoadSceneImmediate(string sceneName)
    {
        StopAllAudio();
        Time.timeScale = 1f;
        CleanupPersistentObjects();
        SceneManager.LoadScene(sceneName);
    }
}
