using UnityEngine;

/// <summary>
/// Visual effects for piano-themed tiles in musical mode
/// Adds glow, scale pulse, and other visual feedback synced to music
/// </summary>
public class PianoTileVisuals : MonoBehaviour
{
    [Header("Visual Settings")]
    [Tooltip("Should this tile pulse with music?")]
    public bool pulseWithMusic = true;
    
    [Tooltip("Should this tile glow when stepped on?")]
    public bool glowOnStep = true;
    
    [Header("Pulse Settings")]
    [Range(0f, 0.2f)]
    public float pulseAmount = 0.05f;
    
    [Range(0f, 10f)]
    public float pulseSpeed = 2f;
    
    [Header("Glow Settings")]
    public Color glowColor = Color.yellow;
    public float glowDuration = 0.5f;
    public float glowIntensity = 2f;
    
    private Vector3 originalScale;
    private Renderer tileRenderer;
    private Material tileMaterial;
    private Color originalColor;
    private bool isGlowing = false;
    private float glowTimer = 0f;
    
    void Start()
    {
        originalScale = transform.localScale;
        tileRenderer = GetComponent<Renderer>();
        
        if (tileRenderer != null)
        {
            // Create a material instance to avoid affecting other tiles
            tileMaterial = tileRenderer.material;
            originalColor = tileMaterial.color;
        }
    }
    
    void Update()
    {
        if (pulseWithMusic && MusicAnalyzer.Instance != null)
        {
            PulseWithMusic();
        }
        
        if (isGlowing)
        {
            UpdateGlow();
        }
    }
    
    /// <summary>
    /// Makes the tile pulse slightly with music intensity
    /// </summary>
    void PulseWithMusic()
    {
        float intensity = MusicAnalyzer.Instance.GetNormalizedIntensity();
        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount * intensity;
        
        transform.localScale = originalScale + Vector3.one * pulse;
    }
    
    /// <summary>
    /// Trigger glow effect when player lands on tile
    /// </summary>
    public void TriggerGlow()
    {
        if (!glowOnStep || tileMaterial == null) return;
        
        isGlowing = true;
        glowTimer = 0f;
    }
    
    /// <summary>
    /// Updates the glow effect over time
    /// </summary>
    void UpdateGlow()
    {
        glowTimer += Time.deltaTime;
        
        if (glowTimer >= glowDuration)
        {
            isGlowing = false;
            tileMaterial.color = originalColor;
            if (tileMaterial.HasProperty("_EmissionColor"))
            {
                tileMaterial.SetColor("_EmissionColor", Color.black);
            }
            return;
        }
        
        // Fade glow
        float t = 1f - (glowTimer / glowDuration);
        Color currentGlow = Color.Lerp(originalColor, glowColor, t);
        tileMaterial.color = currentGlow;
        
        // Add emission if material supports it
        if (tileMaterial.HasProperty("_EmissionColor"))
        {
            tileMaterial.EnableKeyword("_EMISSION");
            tileMaterial.SetColor("_EmissionColor", glowColor * glowIntensity * t);
        }
    }
    
    /// <summary>
    /// Set whether this tile is a white or black piano key
    /// </summary>
    public void SetPianoKeyType(bool isBlackKey)
    {
        if (tileMaterial == null) return;
        
        if (isBlackKey)
        {
            tileMaterial.color = Color.black;
            originalColor = Color.black;
            glowColor = Color.cyan; // Black keys glow cyan
        }
        else
        {
            tileMaterial.color = Color.white;
            originalColor = Color.white;
            glowColor = Color.yellow; // White keys glow yellow
        }
    }
    
    void OnDestroy()
    {
        // Clean up material instance
        if (tileMaterial != null)
        {
            Destroy(tileMaterial);
        }
    }
}
