using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Analyzes audio in real-time to extract music intensity, frequency data, and beat detection
/// Used to drive the musical mode tile generation
/// </summary>
public class MusicAnalyzer : MonoBehaviour
{
    public static MusicAnalyzer Instance;
    
    [Header("Audio Source")]
    public AudioSource musicSource;
    
    [Header("Analysis Settings")]
    [Tooltip("Number of samples to analyze (must be power of 2)")]
    public int sampleSize = 1024;
    
    [Tooltip("Multiplier for intensity values")]
    [Range(1f, 10f)]
    public float intensityMultiplier = 3f;
    
    [Tooltip("Threshold for beat detection")]
    [Range(0.1f, 2f)]
    public float beatThreshold = 0.8f;
    
    [Header("Frequency Bands")]
    [Tooltip("Number of frequency bands to analyze")]
    public int frequencyBands = 8;
    
    // Audio spectrum data
    private float[] samples;
    private float[] frequencyBand;
    private float[] bandBuffer;
    
    // Analysis results
    private float currentIntensity;
    private float averageIntensity;
    private bool isBeat;
    
    // Beat detection
    private float lastBeatTime;
    private float beatCooldown = 0.2f;
    
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
        samples = new float[sampleSize];
        frequencyBand = new float[frequencyBands];
        bandBuffer = new float[frequencyBands];
        
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }
    
    void Update()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            AnalyzeSpectrum();
            CalculateIntensity();
            DetectBeat();
        }
    }
    
    /// <summary>
    /// Analyzes the audio spectrum and divides it into frequency bands
    /// </summary>
    void AnalyzeSpectrum()
    {
        musicSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        
        int count = 0;
        for (int i = 0; i < frequencyBands; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            
            if (i == frequencyBands - 1)
            {
                sampleCount += 2;
            }
            
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            
            average /= count;
            frequencyBand[i] = average * 10 * intensityMultiplier;
        }
        
        // Buffer the frequency bands for smoother transitions
        for (int i = 0; i < frequencyBands; i++)
        {
            if (frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
            }
            else
            {
                bandBuffer[i] -= Time.deltaTime * 2f;
            }
        }
    }
    
    /// <summary>
    /// Calculates overall music intensity from frequency bands
    /// </summary>
    void CalculateIntensity()
    {
        float sum = 0;
        for (int i = 0; i < frequencyBands; i++)
        {
            sum += bandBuffer[i];
        }
        currentIntensity = sum / frequencyBands;
        
        // Calculate running average
        averageIntensity = Mathf.Lerp(averageIntensity, currentIntensity, Time.deltaTime);
    }
    
    /// <summary>
    /// Detects beats in the music
    /// </summary>
    void DetectBeat()
    {
        isBeat = false;
        
        if (Time.time - lastBeatTime > beatCooldown)
        {
            // Check low frequency bands for bass beats
            float bassIntensity = (bandBuffer[0] + bandBuffer[1]) / 2f;
            
            if (bassIntensity > beatThreshold)
            {
                isBeat = true;
                lastBeatTime = Time.time;
            }
        }
    }
    
    // Public getters for other scripts to access
    
    /// <summary>
    /// Returns current music intensity (0-1 range typically, but can go higher)
    /// </summary>
    public float GetIntensity()
    {
        return currentIntensity;
    }
    
    /// <summary>
    /// Returns normalized intensity (0-1 range)
    /// </summary>
    public float GetNormalizedIntensity()
    {
        return Mathf.Clamp01(currentIntensity);
    }
    
    /// <summary>
    /// Returns average intensity over time
    /// </summary>
    public float GetAverageIntensity()
    {
        return averageIntensity;
    }
    
    /// <summary>
    /// Returns true if a beat was detected this frame
    /// </summary>
    public bool IsBeat()
    {
        return isBeat;
    }
    
    /// <summary>
    /// Returns the intensity of a specific frequency band (0-7)
    /// </summary>
    public float GetFrequencyBand(int index)
    {
        if (index >= 0 && index < frequencyBands)
        {
            return bandBuffer[index];
        }
        return 0f;
    }
    
    /// <summary>
    /// Returns if tile should be high jump based on music analysis
    /// Uses multiple factors: overall intensity, bass frequency, and beats
    /// </summary>
    public bool ShouldBeHighJump()
    {
        // High jump if:
        // 1. Current intensity is above average
        // 2. OR it's a beat moment
        // 3. OR bass frequencies are strong
        
        float bassIntensity = (bandBuffer[0] + bandBuffer[1]) / 2f;
        
        return currentIntensity > averageIntensity * 1.2f || 
               isBeat || 
               bassIntensity > beatThreshold * 0.7f;
    }
    
    /// <summary>
    /// Returns recommended tile type based on current music state
    /// 0 = small tile, 1 = big tile
    /// </summary>
    public int GetRecommendedTileType()
    {
        return ShouldBeHighJump() ? 1 : 0;
    }
}
