using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Debug visualizer for music analysis - helps tune the musical mode settings
/// Shows real-time music data on screen
/// </summary>
public class MusicDebugVisualizer : MonoBehaviour
{
    [Header("References")]
    public MusicAnalyzer musicAnalyzer;
    
    [Header("UI Elements")]
    public TextMeshProUGUI intensityText;
    public TextMeshProUGUI beatText;
    public TextMeshProUGUI recommendationText;
    public Image intensityBar;
    
    [Header("Frequency Band Bars (Optional)")]
    public Image[] frequencyBars; // Assign 8 images for 8 frequency bands
    
    [Header("Settings")]
    public bool showDebugInfo = true;
    public Color normalColor = Color.green;
    public Color beatColor = Color.red;
    public Color highIntensityColor = Color.yellow;
    
    void Start()
    {
        if (musicAnalyzer == null)
        {
            musicAnalyzer = FindObjectOfType<MusicAnalyzer>();
        }
    }
    
    void Update()
    {
        if (!showDebugInfo || musicAnalyzer == null) return;
        
        UpdateIntensityDisplay();
        UpdateBeatDisplay();
        UpdateRecommendation();
        UpdateFrequencyBars();
    }
    
    /// <summary>
    /// Updates the intensity text and bar
    /// </summary>
    void UpdateIntensityDisplay()
    {
        float intensity = musicAnalyzer.GetIntensity();
        float normalizedIntensity = musicAnalyzer.GetNormalizedIntensity();
        
        if (intensityText != null)
        {
            intensityText.text = $"Intensity: {intensity:F2} ({normalizedIntensity:F2})";
        }
        
        if (intensityBar != null)
        {
            intensityBar.fillAmount = normalizedIntensity;
            
            // Change color based on intensity
            if (normalizedIntensity > 0.7f)
            {
                intensityBar.color = highIntensityColor;
            }
            else if (musicAnalyzer.IsBeat())
            {
                intensityBar.color = beatColor;
            }
            else
            {
                intensityBar.color = normalColor;
            }
        }
    }
    
    /// <summary>
    /// Updates the beat detection display
    /// </summary>
    void UpdateBeatDisplay()
    {
        if (beatText != null)
        {
            bool isBeat = musicAnalyzer.IsBeat();
            beatText.text = isBeat ? "BEAT!" : "---";
            beatText.color = isBeat ? beatColor : Color.gray;
        }
    }
    
    /// <summary>
    /// Updates the tile recommendation display
    /// </summary>
    void UpdateRecommendation()
    {
        if (recommendationText != null)
        {
            bool shouldBeHigh = musicAnalyzer.ShouldBeHighJump();
            recommendationText.text = shouldBeHigh ? "Next: BIG TILE (High Jump)" : "Next: Small Tile (Low Jump)";
            recommendationText.color = shouldBeHigh ? highIntensityColor : normalColor;
        }
    }
    
    /// <summary>
    /// Updates the frequency band visualization bars
    /// </summary>
    void UpdateFrequencyBars()
    {
        if (frequencyBars == null || frequencyBars.Length == 0) return;
        
        for (int i = 0; i < frequencyBars.Length && i < 8; i++)
        {
            if (frequencyBars[i] != null)
            {
                float bandValue = musicAnalyzer.GetFrequencyBand(i);
                frequencyBars[i].fillAmount = Mathf.Clamp01(bandValue);
                
                // Color code: low frequencies = red, high = blue
                float t = (float)i / 7f;
                frequencyBars[i].color = Color.Lerp(Color.red, Color.blue, t);
            }
        }
    }
    
    /// <summary>
    /// Toggle debug display on/off
    /// </summary>
    public void ToggleDebugDisplay()
    {
        showDebugInfo = !showDebugInfo;
        
        // Hide/show all UI elements
        if (intensityText != null) intensityText.gameObject.SetActive(showDebugInfo);
        if (beatText != null) beatText.gameObject.SetActive(showDebugInfo);
        if (recommendationText != null) recommendationText.gameObject.SetActive(showDebugInfo);
        if (intensityBar != null) intensityBar.gameObject.SetActive(showDebugInfo);
        
        if (frequencyBars != null)
        {
            foreach (var bar in frequencyBars)
            {
                if (bar != null) bar.gameObject.SetActive(showDebugInfo);
            }
        }
    }
    
    /// <summary>
    /// Display debug info in console (for testing without UI)
    /// </summary>
    void OnGUI()
    {
        if (!showDebugInfo || musicAnalyzer == null) return;
        
        // Only show if UI elements aren't assigned (fallback)
        if (intensityText == null && beatText == null)
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"Intensity: {musicAnalyzer.GetIntensity():F2}");
            GUI.Label(new Rect(10, 30, 300, 20), $"Beat: {musicAnalyzer.IsBeat()}");
            GUI.Label(new Rect(10, 50, 300, 20), $"Recommendation: {(musicAnalyzer.ShouldBeHighJump() ? "BIG" : "SMALL")}");
        }
    }
}
