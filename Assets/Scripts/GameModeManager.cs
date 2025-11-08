using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manager for game modes - handles switching between normal and musical mode
/// </summary>
public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;
    
    
    public enum GameMode
    {
        Normal,
        Musical
    }
    
    public GameMode currentMode = GameMode.Musical;
    
    [Header("Generator References")]
    public Generator normalGenerator;
    public MusicalGenerator musicalGenerator;
    
    [Header("Music References")]
    public MusicAnalyzer musicAnalyzer;
    public AudioSource musicalModeAudioSource;
    public AudioClip musicalModeTrack;
    
    [Header("UI")]
    public TextMeshProUGUI modeText;
    public Toggle musicalModeToggle;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Initialize in normal mode
        SetGameMode(currentMode);
        
        // Setup toggle listener if exists
        if (musicalModeToggle != null)
        {
            musicalModeToggle.onValueChanged.AddListener(OnModeToggleChanged);
            musicalModeToggle.isOn = (currentMode == GameMode.Musical);
        }
        
        UpdateModeUI();
    }
    
    /// <summary>
    /// Sets the game mode and activates appropriate generator
    /// </summary>
    public void SetGameMode(GameMode mode)
    {
        currentMode = mode;
        
        switch (mode)
        {
            case GameMode.Normal:
                ActivateNormalMode();
                break;
            case GameMode.Musical:
                ActivateMusicalMode();
                break;
        }
        
        UpdateModeUI();
    }
    
    /// <summary>
    /// Activates normal mode gameplay
    /// </summary>
    void ActivateNormalMode()
    {
        if (normalGenerator != null)
        {
            normalGenerator.enabled = true;
        }
        
        if (musicalGenerator != null)
        {
            musicalGenerator.enabled = false;
        }
        
        if (musicAnalyzer != null)
        {
            musicAnalyzer.enabled = false;
        }
        
        // Use regular background music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBackgroundMusic();
        }
    }
    
    /// <summary>
    /// Activates musical mode gameplay
    /// </summary>
    void ActivateMusicalMode()
    {
        if (normalGenerator != null)
        {
            normalGenerator.enabled = false;
        }
        
        if (musicalGenerator != null)
        {
            musicalGenerator.enabled = true;
        }
        
        if (musicAnalyzer != null)
        {
            musicAnalyzer.enabled = true;
        }
        
        // Play musical mode track
        if (musicalModeAudioSource != null && musicalModeTrack != null)
        {
            musicalModeAudioSource.clip = musicalModeTrack;
            musicalModeAudioSource.Play();
        }
    }
    
    /// <summary>
    /// Toggle between modes
    /// </summary>
    public void ToggleMode()
    {
        if (currentMode == GameMode.Normal)
        {
            SetGameMode(GameMode.Musical);
        }
        else
        {
            SetGameMode(GameMode.Normal);
        }
    }
    
    /// <summary>
    /// Handler for UI toggle
    /// </summary>
    void OnModeToggleChanged(bool isMusicalMode)
    {
        SetGameMode(isMusicalMode ? GameMode.Musical : GameMode.Normal);
    }
    
    /// <summary>
    /// Updates UI to reflect current mode
    /// </summary>
    void UpdateModeUI()
    {
        if (modeText != null)
        {
            modeText.text = currentMode == GameMode.Musical ? "Musical Mode" : "Normal Mode";
        }
    }
    
    /// <summary>
    /// Returns current game mode
    /// </summary>
    public GameMode GetCurrentMode()
    {
        return currentMode;
    }
    
    /// <summary>
    /// Checks if musical mode is active
    /// </summary>
    public bool IsMusicalMode()
    {
        return currentMode == GameMode.Musical;
    }
}
