using UnityEngine;

/// <summary>
/// Checks if the musical mode setup is correct
/// Attach this to any GameObject in your scene to verify setup
/// </summary>
public class SetupChecker : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== MUSICAL MODE SETUP CHECK ===");
        
        // Check for TilesGenerator GameObject
        GameObject tilesGen = GameObject.Find("TilesGenerator");
        if (tilesGen == null)
        {
            Debug.LogError("❌ TilesGenerator GameObject not found in scene!");
            return;
        }
        else
        {
            Debug.Log("✅ TilesGenerator GameObject found");
        }
        
        // Check for Generator component
        Generator gen = tilesGen.GetComponent<Generator>();
        if (gen != null)
        {
            Debug.Log($"✅ Generator component found - Enabled: {gen.enabled}");
        }
        else
        {
            Debug.LogWarning("⚠️ Generator component not found");
        }
        
        // Check for MusicalGenerator component
        MusicalGenerator musGen = tilesGen.GetComponent<MusicalGenerator>();
        if (musGen != null)
        {
            Debug.Log($"✅ MusicalGenerator component found - Enabled: {musGen.enabled}");
            
            // Check if it has required references
            if (musGen.TilePrefab == null)
            {
                Debug.LogError("❌ MusicalGenerator.TilePrefab is not assigned!");
            }
            else
            {
                Debug.Log("✅ TilePrefab is assigned");
            }
        }
        else
        {
            Debug.LogError("❌ MusicalGenerator component not found on TilesGenerator!");
        }
        
        // Check for MelodySequencer
        MelodySequencer melSeq = FindObjectOfType<MelodySequencer>();
        if (melSeq != null)
        {
            Debug.Log($"✅ MelodySequencer found - Loop Melody: {melSeq.loopMelody}");
            Debug.Log($"   Melody Length: {melSeq.GetMelodyLength()} notes");
        }
        else
        {
            Debug.LogError("❌ MelodySequencer not found in scene!");
        }
        
        // Check for GameModeManager
        GameModeManager gmm = FindObjectOfType<GameModeManager>();
        if (gmm != null)
        {
            Debug.Log($"✅ GameModeManager found - Current Mode: {gmm.GetCurrentMode()}");
        }
        else
        {
            Debug.LogWarning("⚠️ GameModeManager not found (optional)");
        }
        
        Debug.Log("=== END SETUP CHECK ===");
    }
}
