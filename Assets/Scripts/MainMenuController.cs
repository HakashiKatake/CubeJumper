using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Main Menu controller for selecting game modes
/// Place this on a GameObject in your MainMenu scene
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Mode Selection Buttons")]
    public Button normalModeButton;
    public Button musicalModeButton;
    public Button uploadMusicModeButton;
    
    [Header("Scene Names")]
    public string normalModeScene = "GameScene_Normal";
    public string musicalModeScene = "GameScene_Musical";
    public string uploadMusicScene = "GameScene_UploadMusic";
    
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public GameObject modeSelectionPanel;
    
    void Start()
    {
        // Setup button listeners
        if (normalModeButton != null)
        {
            normalModeButton.onClick.AddListener(OnNormalModeClick);
        }
        
        if (musicalModeButton != null)
        {
            musicalModeButton.onClick.AddListener(OnMusicalModeClick);
        }
        
        if (uploadMusicModeButton != null)
        {
            uploadMusicModeButton.onClick.AddListener(OnUploadMusicModeClick);
        }
        
        Debug.Log("Main Menu Loaded - Select your game mode!");
    }
    
    /// <summary>
    /// Loads Normal Mode - classic gameplay with random tiles
    /// </summary>
    void OnNormalModeClick()
    {
        Debug.Log("Loading Normal Mode...");
        LoadScene(normalModeScene);
    }
    
    /// <summary>
    /// Loads Musical Mode - predefined melody sequence
    /// </summary>
    void OnMusicalModeClick()
    {
        Debug.Log("Loading Musical Mode...");
        LoadScene(musicalModeScene);
    }
    
    /// <summary>
    /// Loads Upload Music Mode - analyze and play user's own music
    /// </summary>
    void OnUploadMusicModeClick()
    {
        Debug.Log("Loading Upload Music Mode...");
        LoadScene(uploadMusicScene);
    }
    
    /// <summary>
    /// Loads a scene with error handling
    /// </summary>
    void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty!");
            return;
        }
        
        // Stop all audio before loading new scene
        StopAllAudio();
        
        // Check if scene exists in build settings
        if (SceneExists(sceneName))
        {
            // Use SceneTransitionManager if available for cleaner transitions
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' not found in Build Settings! Add it via File > Build Settings > Add Open Scenes");
        }
    }
    
    /// <summary>
    /// Stops all audio sources before scene transition
    /// </summary>
    void StopAllAudio()
    {
        // Find and stop all AudioSource components
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        
        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log($"MainMenu: Stopped audio from {audioSource.gameObject.name}");
            }
        }
    }
    
    /// <summary>
    /// Checks if a scene exists in build settings
    /// </summary>
    bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (scene == sceneName)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Quit application (for standalone builds)
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
