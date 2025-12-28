using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// DEPRECATED: Use SwipeableModeSelector instead for the new swipeable menu system
/// This script is kept for backwards compatibility with old main menu setup
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("DEPRECATED - Use SwipeableModeSelector Instead")]
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private string gameSceneName = "Game";
    
    [Header("New System Compatibility")]
    [Tooltip("If true, this script will be disabled (use SwipeableModeSelector instead)")]
    [SerializeField] private bool useNewSwipeableMenu = true;
    
    private void Start()
    {
        // Check if using new swipeable menu system
        if (useNewSwipeableMenu)
        {
            SwipeableModeSelector swipeSelector = FindObjectOfType<SwipeableModeSelector>();
            if (swipeSelector != null)
            {
                Debug.Log("MainMenu: New swipeable menu detected. Disabling old MainMenu script.");
                this.enabled = false;
                return;
            }
        }
        
        // Make sure HighScoreManager exists
        if (HighScoreManager.Instance == null)
        {
            GameObject highScoreManager = new GameObject("HighScoreManager");
            highScoreManager.AddComponent<HighScoreManager>();
        }
        
        UpdateHighScoreText();
    }
    
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            // OLD BEHAVIOR: Display all three high scores in one text
            // NOTE: In the new swipeable menu, each mode panel shows its own score separately
            int normalScore = HighScoreManager.Instance.GetHighScore(HighScoreManager.GameMode.Normal);
            int musicalScore = HighScoreManager.Instance.GetHighScore(HighScoreManager.GameMode.Musical);
            int uploadScore = HighScoreManager.Instance.GetHighScore(HighScoreManager.GameMode.Upload);
            
            highScoreText.text = $"High Scores:\nNormal: {normalScore} | Musical: {musicalScore} | Upload: {uploadScore}";
        }
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}