using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages color progression in Normal mode
/// Changes tile colors at score milestones: 10, 60, 110, 160, etc.
/// </summary>
public class ColorProgressionManager : MonoBehaviour
{
    [Header("Color Themes")]
    [Tooltip("Array of color themes that will be cycled through as score increases")]
    public ColorTheme[] colorThemes;
    
    [Header("References")]
    [Tooltip("Reference to the player (Cubie) to update current tile color")]
    public GameObject player;
    
    [Tooltip("Reference to the tile prefab used by Generator")]
    public GameObject tilePrefab;
    
    [Header("Debug")]
    public bool showDebugLogs = true;
    
    // Current state
    private int currentThemeIndex = 0;
    private int lastScore = 0;
    private int nextMilestone = 10; // First change at 10
    
    // Singleton pattern
    public static ColorProgressionManager Instance { get; private set; }
    
    [System.Serializable]
    public class ColorTheme
    {
        public string themeName;
        public Color playerStandColor;  // Color when player lands on tile
        public Color tileSpawnColor;    // Color for newly spawned tiles
        
        public ColorTheme(string name, Color playerColor, Color tileColor)
        {
            themeName = name;
            playerStandColor = playerColor;
            tileSpawnColor = tileColor;
        }
    }
    
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
        // Initialize default color themes if not set
        if (colorThemes == null || colorThemes.Length == 0)
        {
            InitializeDefaultThemes();
        }
        
        // Apply first theme
        ApplyCurrentTheme();
        
        if (showDebugLogs)
        {
            Debug.Log($"ColorProgressionManager: Started with {colorThemes.Length} themes. First milestone at {nextMilestone}");
        }
    }
    
    void Update()
    {
        // Only run in Normal mode
        if (GameModeManager.Instance != null && GameModeManager.Instance.IsMusicalMode())
        {
            return;
        }
        
        CheckScoreProgression();
    }
    
    /// <summary>
    /// Check if score has reached next milestone
    /// </summary>
    void CheckScoreProgression()
    {
        // Find score text from player
        if (player != null)
        {
            Cubie cubieScript = player.GetComponent<Cubie>();
            if (cubieScript != null && cubieScript.scoreText != null)
            {
                int currentScore = int.Parse(cubieScript.scoreText.text);
                
                // Check if we've reached a milestone
                if (currentScore >= nextMilestone && currentScore != lastScore)
                {
                    ProgressToNextTheme();
                    lastScore = currentScore;
                }
            }
        }
    }
    
    /// <summary>
    /// Progress to next color theme
    /// </summary>
    void ProgressToNextTheme()
    {
        // Move to next theme
        currentThemeIndex++;
        if (currentThemeIndex >= colorThemes.Length)
        {
            currentThemeIndex = 0; // Loop back to first theme
        }
        
        // Calculate next milestone
        if (nextMilestone == 10)
        {
            nextMilestone = 60; // After 10, next is 60
        }
        else
        {
            nextMilestone += 50; // Then every 50 after that
        }
        
        ApplyCurrentTheme();
        
        if (showDebugLogs)
        {
            Debug.Log($"ColorProgressionManager: Advanced to theme {currentThemeIndex} ({colorThemes[currentThemeIndex].themeName}). Next milestone: {nextMilestone}");
        }
    }
    
    /// <summary>
    /// Apply the current color theme to tiles and player
    /// </summary>
    void ApplyCurrentTheme()
    {
        if (colorThemes == null || colorThemes.Length == 0)
            return;
            
        ColorTheme currentTheme = colorThemes[currentThemeIndex];
        
        // Update tile prefab color (affects newly spawned tiles)
        // Use sharedMaterial for prefabs to avoid "accessing material on prefab" warning
        if (tilePrefab != null)
        {
            Renderer prefabRenderer = tilePrefab.GetComponent<Renderer>();
            if (prefabRenderer != null && prefabRenderer.sharedMaterial != null)
            {
                prefabRenderer.sharedMaterial.SetColor("_Color", currentTheme.tileSpawnColor);
            }
        }
        
        // Update all existing tiles in the scene
        UpdateExistingTiles(currentTheme.tileSpawnColor);
    }
    
    /// <summary>
    /// Update color of all tiles currently in the scene
    /// </summary>
    void UpdateExistingTiles(Color newColor)
    {
        // Find all tiles (objects with TileScript component)
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        
        foreach (TileScript tile in allTiles)
        {
            if (tile == null)
                continue;
                
            Renderer tileRenderer = tile.GetComponent<Renderer>();
            if (tileRenderer != null && tileRenderer.material != null)
            {
                tileRenderer.material.SetColor("_Color", newColor);
            }
        }
        
        if (showDebugLogs && allTiles.Length > 0)
        {
            Debug.Log($"ColorProgressionManager: Updated {allTiles.Length} existing tiles to new color");
        }
    }
    
    /// <summary>
    /// Get the current theme's player stand color (called when player lands on tile)
    /// </summary>
    public Color GetCurrentPlayerStandColor()
    {
        if (colorThemes != null && colorThemes.Length > 0)
        {
            return colorThemes[currentThemeIndex].playerStandColor;
        }
        return Color.red; // Fallback
    }
    
    /// <summary>
    /// Get current theme's tile color (for newly spawned tiles)
    /// </summary>
    public Color GetCurrentTileColor()
    {
        if (colorThemes != null && colorThemes.Length > 0)
        {
            return colorThemes[currentThemeIndex].tileSpawnColor;
        }
        return Color.white; // Fallback
    }
    
    /// <summary>
    /// Initialize default color themes
    /// </summary>
    void InitializeDefaultThemes()
    {
        colorThemes = new ColorTheme[]
        {
            // Theme 0 (0-9 score) - Red/Orange
            new ColorTheme("Sunset", Color.red, new Color(1f, 0.5f, 0f)), // Red player, Orange tiles
            
            // Theme 1 (10-59 score) - Green/Lime
            new ColorTheme("Forest", Color.green, new Color(0.5f, 1f, 0f)), // Green player, Lime tiles
            
            // Theme 2 (60-109 score) - Blue/Cyan
            new ColorTheme("Ocean", Color.blue, Color.cyan), // Blue player, Cyan tiles
            
            // Theme 3 (110-159 score) - Purple/Magenta
            new ColorTheme("Twilight", new Color(0.5f, 0f, 1f), Color.magenta), // Purple player, Magenta tiles
            
            // Theme 4 (160-209 score) - Yellow/Gold
            new ColorTheme("Golden", Color.yellow, new Color(1f, 0.84f, 0f)), // Yellow player, Gold tiles
            
            // Theme 5 (210+ score) - Pink/Rose
            new ColorTheme("Sakura", new Color(1f, 0.41f, 0.71f), new Color(1f, 0.75f, 0.8f)), // Pink player, Rose tiles
        };
        
        if (showDebugLogs)
        {
            Debug.Log($"ColorProgressionManager: Initialized {colorThemes.Length} default themes");
        }
    }
    
    /// <summary>
    /// Reset to first theme (call when restarting game)
    /// </summary>
    public void ResetProgression()
    {
        currentThemeIndex = 0;
        lastScore = 0;
        nextMilestone = 10;
        ApplyCurrentTheme();
        
        if (showDebugLogs)
        {
            Debug.Log("ColorProgressionManager: Reset to first theme");
        }
    }
}
