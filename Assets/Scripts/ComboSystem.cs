using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages combo system for perfect jumps
/// Combo increases when player uses correct jump type for tile
/// Combo breaks when using wrong jump or skipping tiles
/// </summary>
public class ComboSystem : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Text to display current combo count")]
    public TextMeshProUGUI comboText;
    
    [Tooltip("Text to display combo multiplier")]
    public TextMeshProUGUI multiplierText;
    
    [Header("Combo Settings")]
    [Tooltip("How many perfect jumps needed for each multiplier level")]
    public int[] multiplierThresholds = new int[] { 3, 5, 10, 15, 25 };
    
    [Tooltip("Score multipliers for each threshold")]
    public float[] multipliers = new float[] { 1.5f, 2.0f, 3.0f, 4.0f, 5.0f };
    
    [Header("Visual Feedback")]
    [Tooltip("Color for combo text at different levels")]
    public Color[] comboColors = new Color[] 
    { 
        Color.white,        // 0-2 combo
        Color.yellow,       // 3-4 combo
        new Color(1f, 0.5f, 0f), // 5-9 combo (orange)
        new Color(1f, 0f, 0.5f), // 10-14 combo (pink)
        Color.red,          // 15-24 combo
        new Color(0.5f, 0f, 1f)  // 25+ combo (purple)
    };
    
    [Header("Animation")]
    [Tooltip("Scale animation when combo increases")]
    public float scaleAnimDuration = 0.2f;
    public float scaleMultiplier = 1.3f;
    
    [Header("Audio")]
    [Tooltip("Sound to play on combo increase (optional)")]
    public AudioClip comboSound;
    
    [Tooltip("Sound to play on combo break (optional)")]
    public AudioClip comboBreakSound;
    
    private AudioSource audioSource;
    
    // Current combo state
    private int currentCombo = 0;
    private int highestCombo = 0;
    private float currentMultiplier = 1.0f;
    private bool isAnimating = false;
    
    // Singleton
    public static ComboSystem Instance { get; private set; }
    
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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        UpdateComboUI();
    }
    
    /// <summary>
    /// Call when player makes a perfect jump (correct jump type for tile)
    /// </summary>
    public void IncrementCombo()
    {
        currentCombo++;
        
        if (currentCombo > highestCombo)
        {
            highestCombo = currentCombo;
        }
        
        // Update multiplier based on thresholds
        UpdateMultiplier();
        
        // Update UI
        UpdateComboUI();
        
        // Play sound
        if (comboSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(comboSound);
        }
        
        // Animate combo text
        if (!isAnimating && comboText != null)
        {
            StartCoroutine(AnimateComboText());
        }
        
        Debug.Log($"Combo: {currentCombo} | Multiplier: {currentMultiplier}x");
    }
    
    /// <summary>
    /// Call when player breaks combo (wrong jump or skips tile)
    /// </summary>
    /// <param name="reason">Why combo was broken (for feedback)</param>
    public void BreakCombo(string reason = "")
    {
        if (currentCombo > 0)
        {
            Debug.Log($"Combo broken! ({reason}) - Lost {currentCombo} combo");
            
            // Play break sound
            if (comboBreakSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(comboBreakSound);
            }
        }
        
        currentCombo = 0;
        currentMultiplier = 1.0f;
        UpdateComboUI();
    }
    
    /// <summary>
    /// Update multiplier based on current combo
    /// </summary>
    void UpdateMultiplier()
    {
        currentMultiplier = 1.0f;
        
        for (int i = multiplierThresholds.Length - 1; i >= 0; i--)
        {
            if (currentCombo >= multiplierThresholds[i])
            {
                currentMultiplier = multipliers[i];
                break;
            }
        }
    }
    
    /// <summary>
    /// Update combo UI display
    /// </summary>
    void UpdateComboUI()
    {
        // Update combo text
        if (comboText != null)
        {
            if (currentCombo > 0)
            {
                comboText.text = $"Combo: {currentCombo}";
                comboText.gameObject.SetActive(true);
                
                // Update color based on combo level
                comboText.color = GetComboColor();
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
        
        // Update multiplier text
        if (multiplierText != null)
        {
            if (currentMultiplier > 1.0f)
            {
                multiplierText.text = $"{currentMultiplier}x";
                multiplierText.gameObject.SetActive(true);
                multiplierText.color = GetComboColor();
            }
            else
            {
                multiplierText.gameObject.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// Get color for current combo level
    /// </summary>
    Color GetComboColor()
    {
        int colorIndex = 0;
        
        if (currentCombo >= 25) colorIndex = 5;
        else if (currentCombo >= 15) colorIndex = 4;
        else if (currentCombo >= 10) colorIndex = 3;
        else if (currentCombo >= 5) colorIndex = 2;
        else if (currentCombo >= 3) colorIndex = 1;
        
        if (colorIndex < comboColors.Length)
        {
            return comboColors[colorIndex];
        }
        
        return Color.white;
    }
    
    /// <summary>
    /// Animate combo text when combo increases
    /// </summary>
    IEnumerator AnimateComboText()
    {
        if (comboText == null)
            yield break;
            
        isAnimating = true;
        Vector3 originalScale = comboText.transform.localScale;
        Vector3 targetScale = originalScale * scaleMultiplier;
        
        // Scale up
        float elapsed = 0f;
        while (elapsed < scaleAnimDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (scaleAnimDuration / 2f);
            comboText.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        
        // Scale down
        elapsed = 0f;
        while (elapsed < scaleAnimDuration / 2f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (scaleAnimDuration / 2f);
            comboText.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        
        comboText.transform.localScale = originalScale;
        isAnimating = false;
    }
    
    /// <summary>
    /// Get current combo count
    /// </summary>
    public int GetCombo()
    {
        return currentCombo;
    }
    
    /// <summary>
    /// Get current score multiplier
    /// </summary>
    public float GetMultiplier()
    {
        return currentMultiplier;
    }
    
    /// <summary>
    /// Get highest combo achieved this session
    /// </summary>
    public int GetHighestCombo()
    {
        return highestCombo;
    }
    
    /// <summary>
    /// Reset combo system (call on restart)
    /// </summary>
    public void ResetCombo()
    {
        currentCombo = 0;
        highestCombo = 0;
        currentMultiplier = 1.0f;
        UpdateComboUI();
    }
}
