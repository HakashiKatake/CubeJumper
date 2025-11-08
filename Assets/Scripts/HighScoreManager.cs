using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    // Key prefixes for different game modes
    private const string NORMAL_HIGH_SCORE_KEY = "HighScore_Normal";
    private const string MUSICAL_HIGH_SCORE_KEY = "HighScore_Musical";
    private const string UPLOAD_HIGH_SCORE_KEY = "HighScore_Upload";
    
    public static HighScoreManager Instance { get; private set; }
    
    // High scores for each mode
    private int normalModeHighScore = 0;
    private int musicalModeHighScore = 0;
    private int uploadModeHighScore = 0;
    
    // Current game mode
    public enum GameMode
    {
        Normal,
        Musical,
        Upload
    }
    
    private GameMode currentMode = GameMode.Normal;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllHighScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Load all high scores from PlayerPrefs
    /// </summary>
    public void LoadAllHighScores()
    {
        normalModeHighScore = PlayerPrefs.GetInt(NORMAL_HIGH_SCORE_KEY, 0);
        musicalModeHighScore = PlayerPrefs.GetInt(MUSICAL_HIGH_SCORE_KEY, 0);
        uploadModeHighScore = PlayerPrefs.GetInt(UPLOAD_HIGH_SCORE_KEY, 0);
        
        Debug.Log($"Loaded High Scores - Normal: {normalModeHighScore}, Musical: {musicalModeHighScore}, Upload: {uploadModeHighScore}");
    }
    
    /// <summary>
    /// Set the current game mode
    /// </summary>
    public void SetGameMode(GameMode mode)
    {
        currentMode = mode;
        Debug.Log($"HighScoreManager: Game mode set to {mode}");
    }
    
    /// <summary>
    /// Get high score for specific mode
    /// </summary>
    public int GetHighScore(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Normal:
                return normalModeHighScore;
            case GameMode.Musical:
                return musicalModeHighScore;
            case GameMode.Upload:
                return uploadModeHighScore;
            default:
                return 0;
        }
    }
    
    /// <summary>
    /// Get high score for current mode
    /// </summary>
    public int GetCurrentHighScore()
    {
        return GetHighScore(currentMode);
    }
    
    /// <summary>
    /// Check and update high score for current mode
    /// </summary>
    public bool CheckAndUpdateHighScore(int currentScore)
    {
        int currentHighScore = GetCurrentHighScore();
        
        if (currentScore > currentHighScore)
        {
            SetHighScore(currentMode, currentScore);
            SaveHighScore(currentMode);
            Debug.Log($"New High Score for {currentMode}: {currentScore}!");
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Set high score for specific mode
    /// </summary>
    void SetHighScore(GameMode mode, int score)
    {
        switch (mode)
        {
            case GameMode.Normal:
                normalModeHighScore = score;
                break;
            case GameMode.Musical:
                musicalModeHighScore = score;
                break;
            case GameMode.Upload:
                uploadModeHighScore = score;
                break;
        }
    }
    
    /// <summary>
    /// Save high score for specific mode
    /// </summary>
    void SaveHighScore(GameMode mode)
    {
        string key = GetKeyForMode(mode);
        int score = GetHighScore(mode);
        
        PlayerPrefs.SetInt(key, score);
        PlayerPrefs.Save();
        
        Debug.Log($"Saved {mode} high score: {score}");
    }
    
    /// <summary>
    /// Get PlayerPrefs key for mode
    /// </summary>
    string GetKeyForMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.Normal:
                return NORMAL_HIGH_SCORE_KEY;
            case GameMode.Musical:
                return MUSICAL_HIGH_SCORE_KEY;
            case GameMode.Upload:
                return UPLOAD_HIGH_SCORE_KEY;
            default:
                return NORMAL_HIGH_SCORE_KEY;
        }
    }
    
    /// <summary>
    /// Reset high score for specific mode
    /// </summary>
    public void ResetHighScore(GameMode mode)
    {
        SetHighScore(mode, 0);
        SaveHighScore(mode);
        Debug.Log($"Reset {mode} high score");
    }
    
    /// <summary>
    /// Reset all high scores
    /// </summary>
    public void ResetAllHighScores()
    {
        normalModeHighScore = 0;
        musicalModeHighScore = 0;
        uploadModeHighScore = 0;
        
        PlayerPrefs.SetInt(NORMAL_HIGH_SCORE_KEY, 0);
        PlayerPrefs.SetInt(MUSICAL_HIGH_SCORE_KEY, 0);
        PlayerPrefs.SetInt(UPLOAD_HIGH_SCORE_KEY, 0);
        PlayerPrefs.Save();
        
        Debug.Log("Reset all high scores");
    }
}