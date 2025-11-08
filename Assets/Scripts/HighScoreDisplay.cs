using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Displays high score at the start of the game and fades it away when player moves
/// </summary>
public class HighScoreDisplay : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Text component to display high score")]
    public TextMeshProUGUI highScoreText;
    
    [Header("Fade Settings")]
    [Tooltip("How long to wait before starting fade (seconds)")]
    public float displayDuration = 2f;
    
    [Tooltip("Duration of fade out animation (seconds)")]
    public float fadeDuration = 1f;
    
    [Tooltip("Fade out when player moves (jumps on first tile)")]
    public bool fadeOnPlayerMove = true;
    
    [Header("Game Mode")]
    [Tooltip("Which game mode this is for")]
    public HighScoreManager.GameMode gameMode = HighScoreManager.GameMode.Normal;
    
    private CanvasGroup canvasGroup;
    private bool hasFaded = false;
    private bool isPlayerMoving = false;
    private int initialScore = 0;
    
    void Start()
    {
        // Get or add CanvasGroup for fading
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // Set game mode in HighScoreManager
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.SetGameMode(gameMode);
        }
        
        // Display the high score
        DisplayHighScore();
        
        // Start fade timer if not waiting for player movement
        if (!fadeOnPlayerMove)
        {
            StartCoroutine(FadeAfterDelay());
        }
    }
    
    void Update()
    {
        // Check if player has moved (score increased)
        if (fadeOnPlayerMove && !hasFaded && !isPlayerMoving)
        {
            CheckPlayerMovement();
        }
    }
    
    /// <summary>
    /// Display the high score for current mode
    /// </summary>
    void DisplayHighScore()
    {
        if (HighScoreManager.Instance == null)
        {
            Debug.LogWarning("HighScoreManager not found!");
            if (highScoreText != null)
            {
                highScoreText.text = "High Score: 0";
            }
            return;
        }
        
        int highScore = HighScoreManager.Instance.GetHighScore(gameMode);
        
        if (highScoreText != null)
        {
            highScoreText.text = $"High Score: {highScore}";
            Debug.Log($"Displaying {gameMode} high score: {highScore}");
        }
        
        // Make sure it's visible
        canvasGroup.alpha = 1f;
    }
    
    /// <summary>
    /// Check if player has moved (score changed)
    /// </summary>
    void CheckPlayerMovement()
    {
        // Find the score display component
        ScoreDisplay scoreDisplay = FindObjectOfType<ScoreDisplay>();
        
        if (scoreDisplay != null)
        {
            int currentScore = scoreDisplay.GetCurrentScore();
            
            // If score increased from 0, player has moved
            if (currentScore > initialScore)
            {
                isPlayerMoving = true;
                StartCoroutine(FadeOut());
            }
        }
    }
    
    /// <summary>
    /// Public method to trigger fade (can be called from other scripts)
    /// </summary>
    public void TriggerFade()
    {
        if (!hasFaded)
        {
            StartCoroutine(FadeOut());
        }
    }
    
    /// <summary>
    /// Fade after a delay
    /// </summary>
    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeOut());
    }
    
    /// <summary>
    /// Fade out the high score display
    /// </summary>
    IEnumerator FadeOut()
    {
        if (hasFaded) yield break;
        
        hasFaded = true;
        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        
        // Optionally disable the GameObject
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Reset and show high score again (for testing)
    /// </summary>
    public void ResetDisplay()
    {
        hasFaded = false;
        isPlayerMoving = false;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
        DisplayHighScore();
    }
}
