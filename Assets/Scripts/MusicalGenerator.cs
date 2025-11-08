using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates tiles based on music analysis - creates piano-themed tiles
/// that sync with the currently playing music
/// </summary>
public class MusicalGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public GameObject TilePrefab;
    
    [Header("Piano Key Materials")]
    [Tooltip("Material for white piano keys (small tiles)")]
    public Material whitePianoKeyMaterial;
    [Tooltip("Material for black piano keys (big tiles)")]
    public Material blackPianoKeyMaterial;
    
    [Header("Generation Settings")]
    private float xDiff = 1.1f;
    private float yDiffSmall = 0.4f;
    private float yDiffBig = 1.35f;
    
    private float xpos = -2.5f;
    private float Ypos = -4.5f;
    
    private string smallTag = "smallTile";
    private string bigTag = "bigTile";
    
    [Header("Music Sync")]
    [Tooltip("Reference to the music analyzer")]
    public MusicAnalyzer musicAnalyzer;
    
    [Tooltip("Reference to the melody sequencer")]
    public MelodySequencer melodySequencer;
    
    [Tooltip("How responsive to music changes (0-1)")]
    [Range(0f, 1f)]
    public float musicResponsiveness = 0.7f;
    
    [Tooltip("Bias towards high jumps (0 = balanced, 1 = more high jumps)")]
    [Range(0f, 1f)]
    public float highJumpBias = 0f;
    
    // Pattern control
    private Queue<int> upcomingPattern = new Queue<int>();
    private int patternLookahead = 10; // Generate pattern ahead of time
    
    void Start()
    {
        // Find melody sequencer if not assigned
        if (melodySequencer == null)
        {
            melodySequencer = FindObjectOfType<MelodySequencer>();
            if (melodySequencer == null)
            {
                Debug.LogError("MusicalGenerator: No MelodySequencer found in scene!");
            }
        }
        
        // Find music analyzer if not assigned (optional)
        if (musicAnalyzer == null)
        {
            musicAnalyzer = FindObjectOfType<MusicAnalyzer>();
            // This is optional, so no error if not found
        }
        
        Debug.Log("MusicalGenerator: Starting tile generation...");
        
        // Generate initial tiles
        for (int i = 0; i < 7; i++)
        {
            GenerateTiles();
        }
        
        Debug.Log("MusicalGenerator: Initial 7 tiles generated. MusicalGenerator.enabled = " + enabled);
    }
    
    /// <summary>
    /// Main tile generation method - decides between small and big tiles based on music
    /// </summary>
    public void GenerateTiles()
    {
        Debug.Log("MusicalGenerator: GenerateTiles() called");
        
        int tileType = DetermineTileType();
        
        if (tileType == 0)
        {
            GenerateSmallTile();
        }
        else
        {
            GenerateBigTile();
        }
    }
    
    /// <summary>
    /// Determines which type of tile to generate based on music analysis
    /// Returns 0 for small tile (white key), 1 for big tile (black key)
    /// </summary>
    int DetermineTileType()
    {
        if (musicAnalyzer == null || !musicAnalyzer.enabled)
        {
            // Fallback to random if no music analyzer
            return Random.Range(0, 5) <= 2 ? 0 : 1;
        }
        
        // Get music recommendation
        bool shouldBeHigh = musicAnalyzer.ShouldBeHighJump();
        
        // Add some randomness based on responsiveness
        float randomFactor = Random.Range(0f, 1f);
        
        if (randomFactor < musicResponsiveness)
        {
            // Follow music
            return shouldBeHigh ? 1 : 0;
        }
        else
        {
            // Random with bias
            return Random.Range(0f, 1f) < (0.4f + highJumpBias * 0.3f) ? 1 : 0;
        }
    }
    
    /// <summary>
    /// Generates a small tile (white piano key)
    /// </summary>
    void GenerateSmallTile()
    {
        xpos += xDiff;
        Ypos += yDiffSmall;
        
        GameObject tile = Instantiate(TilePrefab, new Vector3(xpos, Ypos, 0), TilePrefab.transform.rotation);
        tile.tag = smallTag;
        
        // Apply white piano key material
        ApplyPianoKeyMaterial(tile, false);
        
        // Assign the next note in the melody to this tile
        AssignNoteToTile(tile);
    }
    
    /// <summary>
    /// Generates a big tile (black piano key)
    /// </summary>
    void GenerateBigTile()
    {
        xpos += xDiff;
        Ypos += yDiffBig;
        
        GameObject tile = Instantiate(TilePrefab, new Vector3(xpos, Ypos, 0), TilePrefab.transform.rotation);
        tile.tag = bigTag;
        
        // Apply black piano key material
        ApplyPianoKeyMaterial(tile, true);
        
        // Assign the next note in the melody to this tile
        AssignNoteToTile(tile);
    }
    
    /// <summary>
    /// Applies piano key themed material to the tile
    /// </summary>
    void ApplyPianoKeyMaterial(GameObject tile, bool isBlackKey)
    {
        Renderer renderer = tile.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (isBlackKey && blackPianoKeyMaterial != null)
            {
                renderer.material = blackPianoKeyMaterial;
            }
            else if (!isBlackKey && whitePianoKeyMaterial != null)
            {
                renderer.material = whitePianoKeyMaterial;
            }
            else
            {
                // Fallback to color if materials not assigned
                renderer.material.color = isBlackKey ? Color.black : Color.white;
            }
        }
    }
    
    /// <summary>
    /// Assigns the next note in the melody sequence to a tile
    /// </summary>
    void AssignNoteToTile(GameObject tile)
    {
        if (melodySequencer == null)
        {
            Debug.LogError("MusicalGenerator: MelodySequencer is null! Cannot assign notes.");
            return;
        }
        
        // Get the next note and assign it to this tile
        AudioClip note = melodySequencer.GetNextNote();
        
        if (note == null)
        {
            Debug.LogError("MusicalGenerator: GetNextNote() returned null!");
            return;
        }
        
        int noteIndex = melodySequencer.GetCurrentNoteIndex() - 1; // -1 because we already advanced
        
        if (noteIndex < 0)
        {
            noteIndex = melodySequencer.GetMelodyLength() - 1;
        }
        
        Debug.Log($"MusicalGenerator: Assigned note {noteIndex} ({note.name}) to tile");
        
        // Store the note in the tile's TileNotePlayer component
        TileNotePlayer notePlayer = tile.GetComponent<TileNotePlayer>();
        if (notePlayer == null)
        {
            notePlayer = tile.AddComponent<TileNotePlayer>();
        }
        
        notePlayer.assignedNote = note;
        notePlayer.noteIndex = noteIndex;
        
        // Also track in the sequencer
        melodySequencer.AssignNoteToTile(tile, noteIndex);
    }
    
    /// <summary>
    /// Get upcoming tile pattern for preview (optional feature)
    /// </summary>
    public Queue<int> GetUpcomingPattern()
    {
        return upcomingPattern;
    }
}
