using UnityEngine;

/// <summary>
/// Component attached to each tile that stores and plays its assigned musical note
/// When the player lands on this tile, it plays its specific note in the melody
/// </summary>
public class TileNotePlayer : MonoBehaviour
{
    [Header("Assigned Note")]
    [Tooltip("The musical note this tile will play")]
    public AudioClip assignedNote;
    
    [Tooltip("Index of this note in the melody sequence")]
    public int noteIndex = -1;
    
    [Header("Playback Settings")]
    [Tooltip("Volume for this note")]
    [Range(0f, 1f)]
    public float volume = 0.8f;
    
    [Tooltip("Should play on landing?")]
    public bool playOnLanding = true;
    
    private bool hasPlayed = false;
    
    /// <summary>
    /// Plays the note assigned to this tile
    /// </summary>
    public void PlayNote()
    {
        if (assignedNote == null)
        {
            Debug.LogWarning("No note assigned to this tile!");
            return;
        }
        
        // Use MelodySequencer's audio source if available
        if (MelodySequencer.Instance != null && MelodySequencer.Instance.noteAudioSource != null)
        {
            AudioSource source = MelodySequencer.Instance.noteAudioSource;
            source.PlayOneShot(assignedNote, volume);
            hasPlayed = true;
        }
        else
        {
            // Fallback: create temporary audio source
            AudioSource.PlayClipAtPoint(assignedNote, transform.position, volume);
            hasPlayed = true;
        }
    }
    
    /// <summary>
    /// Returns whether this tile has played its note
    /// </summary>
    public bool HasPlayed()
    {
        return hasPlayed;
    }
    
    /// <summary>
    /// Reset the played state
    /// </summary>
    public void ResetPlayedState()
    {
        hasPlayed = false;
    }
    
    /// <summary>
    /// Get the note name for display purposes
    /// </summary>
    public string GetNoteName()
    {
        if (assignedNote != null)
        {
            return assignedNote.name;
        }
        return "No Note";
    }
}
