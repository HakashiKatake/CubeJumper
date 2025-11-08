using UnityEngine;

/// <summary>
/// Preset configuration for musical mode - allows saving different settings for different songs
/// </summary>
[CreateAssetMenu(fileName = "MusicalModePreset", menuName = "CubeJumper/Musical Mode Preset")]
public class MusicalModePreset : ScriptableObject
{
    [Header("Preset Info")]
    public string presetName = "Default";
    public string songName = "";
    public AudioClip musicTrack;
    
    [Header("Music Analyzer Settings")]
    [Tooltip("Number of samples to analyze (1024, 2048, 4096)")]
    public int sampleSize = 1024;
    
    [Tooltip("Multiplier for intensity values")]
    [Range(1f, 10f)]
    public float intensityMultiplier = 3f;
    
    [Tooltip("Threshold for beat detection")]
    [Range(0.1f, 2f)]
    public float beatThreshold = 0.8f;
    
    [Header("Generator Settings")]
    [Tooltip("How responsive to music changes (0-1)")]
    [Range(0f, 1f)]
    public float musicResponsiveness = 0.7f;
    
    [Tooltip("Bias towards high jumps (0 = balanced, 1 = more high jumps)")]
    [Range(0f, 1f)]
    public float highJumpBias = 0f;
    
    [Header("Visual Settings")]
    [Tooltip("Should tiles pulse with music?")]
    public bool pulseWithMusic = true;
    
    [Tooltip("Pulse intensity")]
    [Range(0f, 0.2f)]
    public float pulseAmount = 0.05f;
    
    [Tooltip("Glow duration when landing")]
    [Range(0.1f, 2f)]
    public float glowDuration = 0.5f;
    
    [Header("Gameplay Settings")]
    [Tooltip("Game speed multiplier for this song")]
    [Range(0.5f, 3f)]
    public float gameSpeed = 2f;
    
    [Header("Materials")]
    public Material whitePianoKeyMaterial;
    public Material blackPianoKeyMaterial;
    
    /// <summary>
    /// Apply this preset to the musical mode systems
    /// </summary>
    public void ApplyPreset()
    {
        // Apply to MusicAnalyzer
        if (MusicAnalyzer.Instance != null)
        {
            var analyzer = MusicAnalyzer.Instance;
            analyzer.sampleSize = this.sampleSize;
            analyzer.intensityMultiplier = this.intensityMultiplier;
            analyzer.beatThreshold = this.beatThreshold;
            
            // Set the music track
            if (musicTrack != null && analyzer.musicSource != null)
            {
                analyzer.musicSource.clip = musicTrack;
            }
        }
        
        // Apply to MusicalGenerator
        var generator = FindObjectOfType<MusicalGenerator>();
        if (generator != null)
        {
            generator.musicResponsiveness = this.musicResponsiveness;
            generator.highJumpBias = this.highJumpBias;
            
            if (whitePianoKeyMaterial != null)
                generator.whitePianoKeyMaterial = this.whitePianoKeyMaterial;
            if (blackPianoKeyMaterial != null)
                generator.blackPianoKeyMaterial = this.blackPianoKeyMaterial;
        }
        
        // Apply game speed
        Time.timeScale = this.gameSpeed;
        
        Debug.Log($"Applied preset: {presetName} for song: {songName}");
    }
}
