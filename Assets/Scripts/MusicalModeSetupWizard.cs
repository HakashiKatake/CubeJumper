using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Editor wizard to help set up Musical Mode quickly
/// Accessible via Tools → CubeJumper → Musical Mode Setup Wizard
/// </summary>
public class MusicalModeSetupWizard : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/CubeJumper/Musical Mode Setup Wizard")]
    public static void ShowWizard()
    {
        if (EditorUtility.DisplayDialog(
            "Musical Mode Setup Wizard",
            "This wizard will help you set up Musical Mode in your CubeJumper game.\n\n" +
            "It will:\n" +
            "1. Create necessary GameObjects\n" +
            "2. Add required components\n" +
            "3. Set up references\n" +
            "4. Configure default settings\n\n" +
            "Continue?",
            "Yes, Set Up Musical Mode",
            "Cancel"))
        {
            SetupMusicalMode();
        }
    }
    
    [MenuItem("Tools/CubeJumper/Create Musical Mode Preset")]
    public static void CreatePreset()
    {
        // Create a new preset asset
        MusicalModePreset preset = ScriptableObject.CreateInstance<MusicalModePreset>();
        
        // Set default values
        preset.presetName = "New Preset";
        preset.sampleSize = 1024;
        preset.intensityMultiplier = 3f;
        preset.beatThreshold = 0.8f;
        preset.musicResponsiveness = 0.7f;
        preset.highJumpBias = 0f;
        preset.pulseWithMusic = true;
        preset.pulseAmount = 0.05f;
        preset.glowDuration = 0.5f;
        preset.gameSpeed = 2f;
        
        // Save as asset
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Musical Mode Preset",
            "NewMusicalPreset",
            "asset",
            "Choose where to save the preset");
        
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(preset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog(
                "Preset Created",
                $"Musical Mode Preset created at:\n{path}\n\n" +
                "Configure it in the Inspector and add it to your PresetManager!",
                "OK");
            
            // Select the new preset
            Selection.activeObject = preset;
        }
    }
    
    [MenuItem("Tools/CubeJumper/Create Melody Sequence")]
    public static void CreateMelodySequence()
    {
        // Create a new melody sequence asset
        MelodySequence melody = ScriptableObject.CreateInstance<MelodySequence>();
        
        // Set default values
        melody.melodyName = "New Melody";
        melody.loopMelody = true;
        melody.recommendedGameSpeed = 2f;
        
        // Save as asset
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Melody Sequence",
            "NewMelody",
            "asset",
            "Choose where to save the melody sequence");
        
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(melody, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog(
                "Melody Sequence Created",
                $"Melody Sequence created at:\n{path}\n\n" +
                "Now:\n" +
                "1. Select the asset\n" +
                "2. Set the Notes array size\n" +
                "3. Drag your note audio clips in order\n" +
                "4. Assign to MelodySequencer component\n\n" +
                "See MELODY_MODE_SETUP.md for detailed instructions!",
                "OK");
            
            // Select the new melody
            Selection.activeObject = melody;
        }
    }
    
    static void SetupMusicalMode()
    {
        // Step 1: Find or create MusicalModeManager
        GameObject manager = GameObject.Find("MusicalModeManager");
        if (manager == null)
        {
            manager = new GameObject("MusicalModeManager");
            Undo.RegisterCreatedObjectUndo(manager, "Create Musical Mode Manager");
        }
        
        // Step 2: Add MusicAnalyzer
        MusicAnalyzer analyzer = manager.GetComponent<MusicAnalyzer>();
        if (analyzer == null)
        {
            analyzer = manager.AddComponent<MusicAnalyzer>();
        }
        
        // Configure analyzer defaults
        analyzer.sampleSize = 1024;
        analyzer.intensityMultiplier = 3f;
        analyzer.beatThreshold = 0.8f;
        
        // Add AudioSource if needed
        AudioSource musicSource = manager.GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = manager.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        analyzer.musicSource = musicSource;
        
        // Step 3: Add GameModeManager
        GameModeManager gameModeManager = manager.GetComponent<GameModeManager>();
        if (gameModeManager == null)
        {
            gameModeManager = manager.AddComponent<GameModeManager>();
        }
        
        gameModeManager.musicAnalyzer = analyzer;
        gameModeManager.musicalModeAudioSource = musicSource;
        
        // Step 4: Find existing generators
        Generator normalGen = FindObjectOfType<Generator>();
        if (normalGen != null)
        {
            gameModeManager.normalGenerator = normalGen;
        }
        
        // Step 5: Create or find MusicalGenerator
        GameObject musicalGenObj = GameObject.Find("MusicalTilesGenerator");
        MusicalGenerator musicalGen;
        
        if (musicalGenObj == null && normalGen != null)
        {
            // Duplicate the normal generator
            musicalGenObj = Instantiate(normalGen.gameObject);
            musicalGenObj.name = "MusicalTilesGenerator";
            Undo.RegisterCreatedObjectUndo(musicalGenObj, "Create Musical Generator");
            
            // Remove old generator, add new one
            DestroyImmediate(musicalGenObj.GetComponent<Generator>());
            musicalGen = musicalGenObj.AddComponent<MusicalGenerator>();
            
            // Copy tile prefab reference
            musicalGen.TilePrefab = normalGen.TilePrefab;
        }
        else if (musicalGenObj != null)
        {
            musicalGen = musicalGenObj.GetComponent<MusicalGenerator>();
            if (musicalGen == null)
            {
                musicalGen = musicalGenObj.AddComponent<MusicalGenerator>();
            }
        }
        else
        {
            // Create from scratch
            musicalGenObj = new GameObject("MusicalTilesGenerator");
            musicalGen = musicalGenObj.AddComponent<MusicalGenerator>();
            Undo.RegisterCreatedObjectUndo(musicalGenObj, "Create Musical Generator");
        }
        
        // Configure musical generator
        musicalGen.musicAnalyzer = analyzer;
        musicalGen.musicResponsiveness = 0.7f;
        musicalGen.highJumpBias = 0f;
        gameModeManager.musicalGenerator = musicalGen;
        
        // Step 6: Add MusicDebugVisualizer (optional)
        MusicDebugVisualizer debugVis = manager.GetComponent<MusicDebugVisualizer>();
        if (debugVis == null)
        {
            debugVis = manager.AddComponent<MusicDebugVisualizer>();
        }
        debugVis.musicAnalyzer = analyzer;
        debugVis.showDebugInfo = false; // Disabled by default
        
        // Step 7: Add PresetManager
        PresetManager presetManager = manager.GetComponent<PresetManager>();
        if (presetManager == null)
        {
            presetManager = manager.AddComponent<PresetManager>();
        }
        
        // Step 8: Add MelodySequencer
        MelodySequencer melodySequencer = manager.GetComponent<MelodySequencer>();
        if (melodySequencer == null)
        {
            melodySequencer = manager.AddComponent<MelodySequencer>();
        }
        
        // Configure melody sequencer
        melodySequencer.loopMelody = true;
        melodySequencer.noteVolume = 0.8f;
        
        // Connect to musical generator
        if (musicalGen != null)
        {
            musicalGen.melodySequencer = melodySequencer;
        }
        
        // Step 9: Mark scene dirty
        #if UNITY_EDITOR
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        #endif
        
        // Step 9: Select the manager
        Selection.activeGameObject = manager;
        
        // Step 10: Show completion message
        EditorUtility.DisplayDialog(
            "Setup Complete!",
            "Musical Melody Mode has been set up successfully!\n\n" +
            "Next steps:\n" +
            "1. Create a Melody Sequence (Right-click → Create → CubeJumper → Melody Sequence)\n" +
            "2. Add your note audio clips to the sequence\n" +
            "3. Assign the sequence to MelodySequencer\n" +
            "4. Create piano key materials (optional)\n" +
            "5. Enable Musical Mode and play!\n\n" +
            "Check MELODY_MODE_SETUP.md for detailed instructions!\n\n" +
            "The MusicalModeManager is now selected in the hierarchy.",
            "OK");
    }
    
    [MenuItem("Tools/CubeJumper/Add Piano Visuals to Tile")]
    public static void AddPianoVisualsToTile()
    {
        GameObject selected = Selection.activeGameObject;
        
        if (selected == null)
        {
            EditorUtility.DisplayDialog(
                "No GameObject Selected",
                "Please select your Tile Prefab in the hierarchy or project, then run this again.",
                "OK");
            return;
        }
        
        PianoTileVisuals visuals = selected.GetComponent<PianoTileVisuals>();
        if (visuals != null)
        {
            EditorUtility.DisplayDialog(
                "Already Added",
                "This GameObject already has PianoTileVisuals component!",
                "OK");
            return;
        }
        
        visuals = selected.AddComponent<PianoTileVisuals>();
        visuals.pulseWithMusic = true;
        visuals.glowOnStep = true;
        visuals.pulseAmount = 0.05f;
        visuals.pulseSpeed = 2f;
        visuals.glowColor = Color.yellow;
        visuals.glowDuration = 0.5f;
        visuals.glowIntensity = 2f;
        
        EditorUtility.DisplayDialog(
            "Component Added",
            "PianoTileVisuals has been added to " + selected.name + "!\n\n" +
            "Configure the settings in the Inspector.",
            "OK");
    }
    
    [MenuItem("Tools/CubeJumper/Documentation/Open Melody Mode Setup")]
    public static void OpenMelodySetup()
    {
        string path = "Assets/Scripts/MELODY_MODE_SETUP.md";
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
    }
    
    [MenuItem("Tools/CubeJumper/Documentation/Open Quick Start")]
    public static void OpenQuickStart()
    {
        string path = "Assets/Scripts/QUICK_START.md";
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
    }
    
    [MenuItem("Tools/CubeJumper/Documentation/Open Full Setup Guide")]
    public static void OpenSetupGuide()
    {
        string path = "Assets/Scripts/MUSICAL_MODE_SETUP.md";
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
    }
    
    [MenuItem("Tools/CubeJumper/Documentation/Open Architecture Guide")]
    public static void OpenArchitecture()
    {
        string path = "Assets/Scripts/ARCHITECTURE.md";
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
    }
    
    [MenuItem("Tools/CubeJumper/Documentation/Open Preset Examples")]
    public static void OpenPresetExamples()
    {
        string path = "Assets/Scripts/PRESET_EXAMPLES.md";
        AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
    }
#endif
}
