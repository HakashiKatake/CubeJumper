using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the melody sequence for musical mode
/// Each tile plays a specific note in the song's melody
/// As player progresses, they play the song note by note
/// </summary>
public class MelodySequencer : MonoBehaviour
{
    public static MelodySequencer Instance;
    
    [Header("Melody Notes")]
    [Tooltip("Array of audio clips representing the melody sequence")]
    public AudioClip[] melodyNotes;
    
    [Tooltip("Or use a Melody Sequence asset")]
    public MelodySequence melodySequence;
    
    [Header("Audio Source")]
    public AudioSource noteAudioSource;
    
    [Header("Melody Settings")]
    [Tooltip("Should the melody loop when it reaches the end?")]
    public bool loopMelody = true;
    
    [Tooltip("Volume for note playback")]
    [Range(0f, 1f)]
    public float noteVolume = 0.8f;
    
    [Tooltip("Pitch variation for variety")]
    [Range(0f, 0.2f)]
    public float pitchVariation = 0f;
    
    // Current position in the melody
    private int currentNoteIndex = 0;
    
    // Track which tiles have played which notes for visualization
    private Dictionary<GameObject, int> tileNoteMap = new Dictionary<GameObject, int>();
    
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
        if (noteAudioSource == null)
        {
            noteAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        noteAudioSource.volume = noteVolume;
        noteAudioSource.playOnAwake = false;
        
        // Load melody from sequence asset if provided
        if (melodySequence != null && melodySequence.IsValid())
        {
            melodyNotes = melodySequence.notes;
            loopMelody = melodySequence.loopMelody;
        }
    }
    
    /// <summary>
    /// Gets the next note in the melody sequence
    /// </summary>
    public AudioClip GetNextNote()
    {
        if (melodyNotes == null || melodyNotes.Length == 0)
        {
            Debug.LogWarning("No melody notes assigned!");
            return null;
        }
        
        AudioClip note = melodyNotes[currentNoteIndex];
        
        // Advance to next note
        currentNoteIndex++;
        
        // Loop back if needed
        if (currentNoteIndex >= melodyNotes.Length)
        {
            if (loopMelody)
            {
                currentNoteIndex = 0;
            }
            else
            {
                currentNoteIndex = melodyNotes.Length - 1; // Stay on last note
            }
        }
        
        return note;
    }
    
    /// <summary>
    /// Plays a specific note from the melody
    /// </summary>
    public void PlayNote(AudioClip note)
    {
        if (noteAudioSource == null || note == null) return;
        
        noteAudioSource.clip = note;
        noteAudioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        noteAudioSource.Play();
    }
    
    /// <summary>
    /// Plays the next note in sequence
    /// </summary>
    public void PlayNextNote()
    {
        AudioClip note = GetNextNote();
        PlayNote(note);
    }
    
    /// <summary>
    /// Gets the current note index in the melody
    /// </summary>
    public int GetCurrentNoteIndex()
    {
        return currentNoteIndex;
    }
    
    /// <summary>
    /// Resets the melody back to the beginning
    /// </summary>
    public void ResetMelody()
    {
        currentNoteIndex = 0;
        tileNoteMap.Clear();
    }
    
    /// <summary>
    /// Associates a tile with a specific note index for tracking
    /// </summary>
    public void AssignNoteToTile(GameObject tile, int noteIndex)
    {
        if (!tileNoteMap.ContainsKey(tile))
        {
            tileNoteMap[tile] = noteIndex;
        }
    }
    
    /// <summary>
    /// Gets the note index assigned to a specific tile
    /// </summary>
    public int GetTileNoteIndex(GameObject tile)
    {
        if (tileNoteMap.ContainsKey(tile))
        {
            return tileNoteMap[tile];
        }
        return -1;
    }
    
    /// <summary>
    /// Peek at what the next note will be without advancing
    /// </summary>
    public AudioClip PeekNextNote()
    {
        if (melodyNotes == null || melodyNotes.Length == 0)
            return null;
            
        return melodyNotes[currentNoteIndex];
    }
    
    /// <summary>
    /// Get note at specific index
    /// </summary>
    public AudioClip GetNoteAtIndex(int index)
    {
        if (melodyNotes == null || melodyNotes.Length == 0)
            return null;
            
        if (index < 0 || index >= melodyNotes.Length)
            return null;
            
        return melodyNotes[index];
    }
    
    /// <summary>
    /// Get total number of notes in melody
    /// </summary>
    public int GetMelodyLength()
    {
        return melodyNotes != null ? melodyNotes.Length : 0;
    }
    
    /// <summary>
    /// Calculate progress through the melody (0-1)
    /// </summary>
    public float GetMelodyProgress()
    {
        if (melodyNotes == null || melodyNotes.Length == 0)
            return 0f;
            
        return (float)currentNoteIndex / melodyNotes.Length;
    }
    
    /// <summary>
    /// Set melody from external source (for dynamic melodies)
    /// </summary>
    public void SetMelody(AudioClip[] newMelody)
    {
        melodyNotes = newMelody;
        ResetMelody();
    }
    
    /// <summary>
    /// Set dynamic melody from uploaded audio analysis
    /// </summary>
    public void SetDynamicMelody(AudioClip[] detectedNotes)
    {
        Debug.Log($"MelodySequencer: Setting dynamic melody with {detectedNotes.Length} notes");
        melodyNotes = detectedNotes;
        loopMelody = false; // Don't loop extracted songs by default
        ResetMelody();
    }
}
