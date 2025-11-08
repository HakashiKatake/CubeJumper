using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Common game controller for all game modes
/// Handles pause, restart, back to menu functionality
/// </summary>
public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenu;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public string mainMenuSceneName = "MainMenu";
    
    private bool isPaused = false;
    
    void Start()
    {
        // Setup button listeners
        if (pauseButton != null)
            pauseButton.onClick.AddListener(PauseGame);
            
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
            
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        
        // Hide pause menu initially
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }
    
    void Update()
    {
        // Toggle pause with escape key
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenu != null)
            pauseMenu.SetActive(true);
            
        Debug.Log("Game Paused");
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
            
        Debug.Log("Game Resumed");
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        StopAllAudio();
        
        // Use SceneTransitionManager if available, otherwise direct load
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        Debug.Log("Game Restarted");
    }
    
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        StopAllAudio();
        
        // Use SceneTransitionManager if available, otherwise direct load
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(mainMenuSceneName);
        }
        else
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        
        Debug.Log("Returning to Main Menu");
    }
    
    /// <summary>
    /// Stops all audio sources in the scene before transitioning
    /// </summary>
    void StopAllAudio()
    {
        // Find and stop all AudioSource components in the scene
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log($"Stopped audio from: {audioSource.gameObject.name}");
            }
        }
    }
    
    void OnDestroy()
    {
        // Ensure audio is stopped when this controller is destroyed
        StopAllAudio();
    }
}
