using UnityEngine;

/// <summary>
/// Quick test script for music upload feature
/// Attach to any GameObject and assign a test audio file
/// </summary>
public class MusicUploadTester : MonoBehaviour
{
    [Header("Test Audio")]
    [Tooltip("Drag an audio file from your Project window here to test")]
    public AudioClip testAudioClip;
    
    [Header("References")]
    public AudioMelodyExtractor melodyExtractor;
    public MelodySequencer melodySequencer;
    
    [Header("Test Controls")]
    [Tooltip("Press this key to start analysis")]
    public KeyCode analyzeKey = KeyCode.Space;
    
    void Start()
    {
        if (melodyExtractor == null)
        {
            melodyExtractor = FindObjectOfType<AudioMelodyExtractor>();
        }
        
        if (melodySequencer == null)
        {
            melodySequencer = FindObjectOfType<MelodySequencer>();
        }
        
        Debug.Log("MusicUploadTester: Press SPACE to analyze test audio clip");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(analyzeKey))
        {
            TestAnalyze();
        }
    }
    
    void TestAnalyze()
    {
        if (testAudioClip == null)
        {
            Debug.LogError("No test audio clip assigned!");
            return;
        }
        
        if (melodyExtractor == null)
        {
            Debug.LogError("AudioMelodyExtractor not found!");
            return;
        }
        
        Debug.Log($"Starting analysis of: {testAudioClip.name}");
        melodyExtractor.AnalyzeAudio(testAudioClip);
    }
}
