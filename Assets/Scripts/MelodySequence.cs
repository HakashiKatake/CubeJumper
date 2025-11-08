using UnityEngine;

/// <summary>
/// Scriptable Object to store a melody sequence
/// Create via: Right-click -> Create -> CubeJumper -> Melody Sequence
/// </summary>
[CreateAssetMenu(fileName = "NewMelody", menuName = "CubeJumper/Melody Sequence")]
public class MelodySequence : ScriptableObject
{
    [Header("Melody Info")]
    public string melodyName = "New Melody";
    public string songName = "";
    
    [TextArea(3, 10)]
    public string description = "Description of this melody...";
    
    [Header("Note Sequence")]
    [Tooltip("Array of audio clips in the order they should be played")]
    public AudioClip[] notes;
    
    [Header("Settings")]
    [Tooltip("Should the melody loop when it reaches the end?")]
    public bool loopMelody = true;
    
    [Tooltip("Recommended game speed for this melody")]
    [Range(0.5f, 3f)]
    public float recommendedGameSpeed = 2f;
    
    /// <summary>
    /// Get the total number of notes in this melody
    /// </summary>
    public int GetNoteCount()
    {
        return notes != null ? notes.Length : 0;
    }
    
    /// <summary>
    /// Validate that all notes are assigned
    /// </summary>
    public bool IsValid()
    {
        if (notes == null || notes.Length == 0)
            return false;
            
        foreach (var note in notes)
        {
            if (note == null)
                return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Get note at specific index
    /// </summary>
    public AudioClip GetNoteAt(int index)
    {
        if (notes == null || index < 0 || index >= notes.Length)
            return null;
            
        return notes[index];
    }
}
