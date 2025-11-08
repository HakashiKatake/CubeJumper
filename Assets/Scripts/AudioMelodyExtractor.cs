using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extracts melody notes from uploaded audio files using pitch detection
/// Maps detected frequencies to piano note samples
/// </summary>
public class AudioMelodyExtractor : MonoBehaviour
{
    public static AudioMelodyExtractor Instance;
    
    [Header("Audio Analysis Settings")]
    [Tooltip("The uploaded audio clip to analyze")]
    public AudioClip uploadedAudio;
    
    [Tooltip("Audio source for playback")]
    public AudioSource audioSource;
    
    [Tooltip("Sample size for FFT analysis (must be power of 2)")]
    public int sampleSize = 4096;
    
    [Tooltip("How often to sample the audio (in seconds)")]
    [Range(0.05f, 1f)]
    public float sampleInterval = 0.2f;
    
    [Tooltip("Minimum volume threshold to detect a note")]
    [Range(0.001f, 0.1f)]
    public float volumeThreshold = 0.01f;
    
    [Header("Note Detection")]
    [Tooltip("Reference frequency for A4 (standard is 440 Hz)")]
    public float referenceFrequency = 440f;
    
    // Extracted melody data
    private List<string> detectedNotes = new List<string>();
    private List<AudioClip> assignedNoteClips = new List<AudioClip>();
    
    // Note samples library
    private Dictionary<string, AudioClip> noteSamplesLibrary = new Dictionary<string, AudioClip>();
    
    // Analysis state
    private bool isAnalyzing = false;
    private bool analysisComplete = false;
    
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
        LoadNoteSamples();
    }
    
    /// <summary>
    /// Loads all piano note samples from the Sounds folder
    /// </summary>
    void LoadNoteSamples()
    {
        Debug.Log("AudioMelodyExtractor: Loading note samples...");
        
        // Load all audio clips from Resources/Sounds folder
        // You'll need to move your sound files to Resources/Sounds for this to work
        AudioClip[] allSounds = Resources.LoadAll<AudioClip>("Sounds");
        
        foreach (AudioClip clip in allSounds)
        {
            string noteName = clip.name.ToLower();
            
            // Parse note names like "c4", "d5", "e3", etc.
            if (IsValidNoteName(noteName))
            {
                noteSamplesLibrary[noteName] = clip;
                Debug.Log($"Loaded note: {noteName}");
            }
        }
        
        Debug.Log($"AudioMelodyExtractor: Loaded {noteSamplesLibrary.Count} note samples");
    }
    
    /// <summary>
    /// Starts analyzing the uploaded audio file
    /// </summary>
    public void AnalyzeAudio(AudioClip clip)
    {
        if (isAnalyzing)
        {
            Debug.LogWarning("Already analyzing audio!");
            return;
        }
        
        uploadedAudio = clip;
        StartCoroutine(AnalyzeAudioCoroutine());
    }
    
    /// <summary>
    /// Coroutine that analyzes the audio and extracts melody
    /// </summary>
    IEnumerator AnalyzeAudioCoroutine()
    {
        isAnalyzing = true;
        analysisComplete = false;
        detectedNotes.Clear();
        assignedNoteClips.Clear();
        
        Debug.Log("Starting audio analysis...");
        
        if (uploadedAudio == null)
        {
            Debug.LogError("No audio clip to analyze!");
            isAnalyzing = false;
            yield break;
        }
        
        // Get audio data
        float[] audioData = new float[uploadedAudio.samples * uploadedAudio.channels];
        uploadedAudio.GetData(audioData, 0);
        
        int sampleRate = uploadedAudio.frequency;
        int channels = uploadedAudio.channels;
        
        // Process audio in chunks
        int samplesPerInterval = Mathf.FloorToInt(sampleInterval * sampleRate);
        int totalSamples = audioData.Length / channels;
        
        for (int i = 0; i < totalSamples; i += samplesPerInterval)
        {
            // Get chunk of audio
            int chunkSize = Mathf.Min(sampleSize, totalSamples - i);
            float[] chunk = new float[chunkSize];
            
            // Convert stereo to mono if needed
            for (int j = 0; j < chunkSize; j++)
            {
                int sampleIndex = (i + j) * channels;
                if (sampleIndex < audioData.Length)
                {
                    float sample = 0;
                    for (int c = 0; c < channels; c++)
                    {
                        sample += audioData[sampleIndex + c];
                    }
                    chunk[j] = sample / channels;
                }
            }
            
            // Detect pitch in this chunk
            float frequency = DetectPitch(chunk, sampleRate);
            
            if (frequency > 0)
            {
                string noteName = FrequencyToNoteName(frequency);
                AudioClip noteClip = GetClosestNoteClip(noteName);
                
                if (noteClip != null)
                {
                    detectedNotes.Add(noteName);
                    assignedNoteClips.Add(noteClip);
                    Debug.Log($"Detected: {noteName} ({frequency:F2} Hz) at {i / (float)sampleRate:F2}s");
                }
            }
            
            // Yield occasionally to prevent freezing
            if (i % (samplesPerInterval * 10) == 0)
            {
                yield return null;
            }
        }
        
        Debug.Log($"Analysis complete! Detected {detectedNotes.Count} notes");
        isAnalyzing = false;
        analysisComplete = true;
        
        // Create melody sequence from detected notes
        CreateMelodySequenceFromDetectedNotes();
    }
    
    /// <summary>
    /// Detects the dominant pitch/frequency in an audio chunk using autocorrelation
    /// </summary>
    float DetectPitch(float[] audioData, int sampleRate)
    {
        // Calculate RMS (volume)
        float rms = 0;
        for (int i = 0; i < audioData.Length; i++)
        {
            rms += audioData[i] * audioData[i];
        }
        rms = Mathf.Sqrt(rms / audioData.Length);
        
        // Ignore silent parts
        if (rms < volumeThreshold)
        {
            return -1;
        }
        
        // Simple autocorrelation pitch detection
        int minPeriod = sampleRate / 1000; // Max ~1000 Hz
        int maxPeriod = sampleRate / 60;   // Min ~60 Hz
        
        float bestCorrelation = 0;
        int bestPeriod = 0;
        
        for (int period = minPeriod; period < maxPeriod && period < audioData.Length / 2; period++)
        {
            float correlation = 0;
            for (int i = 0; i < audioData.Length - period; i++)
            {
                correlation += audioData[i] * audioData[i + period];
            }
            
            if (correlation > bestCorrelation)
            {
                bestCorrelation = correlation;
                bestPeriod = period;
            }
        }
        
        if (bestPeriod > 0)
        {
            return (float)sampleRate / bestPeriod;
        }
        
        return -1;
    }
    
    /// <summary>
    /// Converts frequency (Hz) to musical note name
    /// </summary>
    string FrequencyToNoteName(float frequency)
    {
        // Calculate number of half steps from A4 (440 Hz)
        float halfStepsFromA4 = 12f * Mathf.Log(frequency / referenceFrequency, 2f);
        int roundedHalfSteps = Mathf.RoundToInt(halfStepsFromA4);
        
        // A4 is MIDI note 69
        int midiNote = 69 + roundedHalfSteps;
        
        // Convert MIDI note to note name
        string[] noteNames = { "c", "c#", "d", "d#", "e", "f", "f#", "g", "g#", "a", "a#", "b" };
        int octave = (midiNote / 12) - 1;
        int noteIndex = midiNote % 12;
        
        string noteName = noteNames[noteIndex] + octave;
        
        return noteName;
    }
    
    /// <summary>
    /// Gets the closest available note clip from library
    /// </summary>
    AudioClip GetClosestNoteClip(string targetNote)
    {
        // Try exact match first
        if (noteSamplesLibrary.ContainsKey(targetNote))
        {
            return noteSamplesLibrary[targetNote];
        }
        
        // Try without sharp/flat
        string baseNote = targetNote.Replace("#", "_1").Replace("_1", "");
        if (noteSamplesLibrary.ContainsKey(baseNote))
        {
            return noteSamplesLibrary[baseNote];
        }
        
        // Try with _1 suffix (for sharp notes)
        if (targetNote.Contains("#"))
        {
            string altNote = targetNote.Replace("#", "_1");
            if (noteSamplesLibrary.ContainsKey(altNote))
            {
                return noteSamplesLibrary[altNote];
            }
        }
        
        Debug.LogWarning($"No sample found for note: {targetNote}");
        return null;
    }
    
    /// <summary>
    /// Creates a MelodySequence from detected notes
    /// </summary>
    void CreateMelodySequenceFromDetectedNotes()
    {
        if (assignedNoteClips.Count == 0)
        {
            Debug.LogError("No notes detected to create melody!");
            return;
        }
        
        // Find or create MelodySequencer
        MelodySequencer sequencer = FindObjectOfType<MelodySequencer>();
        if (sequencer != null)
        {
            // Create a new melody sequence from detected notes
            sequencer.SetDynamicMelody(assignedNoteClips.ToArray());
            Debug.Log($"Created dynamic melody with {assignedNoteClips.Count} notes");
        }
    }
    
    /// <summary>
    /// Checks if a string is a valid note name
    /// </summary>
    bool IsValidNoteName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        
        // Check for pattern like "c4", "d5", "e3", etc.
        if (name.Length >= 2)
        {
            char note = name[0];
            bool isNote = (note >= 'a' && note <= 'g');
            bool hasNumber = char.IsDigit(name[name.Length - 1]);
            
            return isNote && hasNumber;
        }
        
        return false;
    }
    
    // Public getters
    public List<string> GetDetectedNotes() => detectedNotes;
    public List<AudioClip> GetAssignedNoteClips() => assignedNoteClips;
    public bool IsAnalysisComplete() => analysisComplete;
    public bool IsAnalyzing() => isAnalyzing;
}
