using UnityEngine;
using TMPro;

/// <summary>
/// Manages score display and provides access to current score
/// Attach this to the same GameObject as your score TextMeshProUGUI
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    [Header("Score Text")]
    [Tooltip("The TextMeshProUGUI component showing the score")]
    public TextMeshProUGUI scoreText;
    
    private int currentScore = 0;
    
    void Start()
    {
        // Auto-find score text if not assigned
        if (scoreText == null)
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }
        
        // Initialize score to 0
        if (scoreText != null)
        {
            scoreText.text = "0";
        }
    }
    
    void Update()
    {
        // Keep tracking current score
        if (scoreText != null && !string.IsNullOrEmpty(scoreText.text))
        {
            if (int.TryParse(scoreText.text, out int score))
            {
                currentScore = score;
            }
        }
    }
    
    /// <summary>
    /// Get the current score value
    /// </summary>
    public int GetCurrentScore()
    {
        // Try to parse from text if available
        if (scoreText != null && !string.IsNullOrEmpty(scoreText.text))
        {
            if (int.TryParse(scoreText.text, out int score))
            {
                currentScore = score;
            }
        }
        
        return currentScore;
    }
    
    /// <summary>
    /// Set the score
    /// </summary>
    public void SetScore(int score)
    {
        currentScore = score;
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    
    /// <summary>
    /// Add to the score
    /// </summary>
    public void AddScore(int amount)
    {
        SetScore(currentScore + amount);
    }
    
    /// <summary>
    /// Reset score to 0
    /// </summary>
    public void ResetScore()
    {
        SetScore(0);
    }
}
