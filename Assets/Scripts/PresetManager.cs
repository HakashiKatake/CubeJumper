using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages loading and switching between different musical mode presets
/// </summary>
public class PresetManager : MonoBehaviour
{
    [Header("Available Presets")]
    public MusicalModePreset[] presets;
    
    [Header("UI")]
    public TMP_Dropdown presetDropdown;
    public TextMeshProUGUI currentPresetText;
    
    private int currentPresetIndex = 0;
    
    void Start()
    {
        PopulateDropdown();
        
        // Load default preset
        if (presets != null && presets.Length > 0)
        {
            LoadPreset(0);
        }
    }
    
    /// <summary>
    /// Populates the dropdown with available presets
    /// </summary>
    void PopulateDropdown()
    {
        if (presetDropdown == null || presets == null) return;
        
        presetDropdown.ClearOptions();
        
        var options = new System.Collections.Generic.List<string>();
        foreach (var preset in presets)
        {
            if (preset != null)
            {
                options.Add(preset.presetName);
            }
        }
        
        presetDropdown.AddOptions(options);
        presetDropdown.onValueChanged.AddListener(OnPresetSelected);
    }
    
    /// <summary>
    /// Called when user selects a preset from dropdown
    /// </summary>
    void OnPresetSelected(int index)
    {
        LoadPreset(index);
    }
    
    /// <summary>
    /// Loads and applies a specific preset
    /// </summary>
    public void LoadPreset(int index)
    {
        if (presets == null || index < 0 || index >= presets.Length)
        {
            Debug.LogWarning("Invalid preset index: " + index);
            return;
        }
        
        var preset = presets[index];
        if (preset == null)
        {
            Debug.LogWarning("Preset at index " + index + " is null");
            return;
        }
        
        currentPresetIndex = index;
        preset.ApplyPreset();
        
        // Update UI
        if (currentPresetText != null)
        {
            currentPresetText.text = $"Current: {preset.presetName}";
        }
        
        // Start the music if in musical mode
        if (GameModeManager.Instance != null && GameModeManager.Instance.IsMusicalMode())
        {
            if (MusicAnalyzer.Instance != null && MusicAnalyzer.Instance.musicSource != null)
            {
                MusicAnalyzer.Instance.musicSource.Play();
            }
        }
    }
    
    /// <summary>
    /// Load next preset in the list
    /// </summary>
    public void NextPreset()
    {
        int nextIndex = (currentPresetIndex + 1) % presets.Length;
        LoadPreset(nextIndex);
        
        if (presetDropdown != null)
        {
            presetDropdown.value = nextIndex;
        }
    }
    
    /// <summary>
    /// Load previous preset in the list
    /// </summary>
    public void PreviousPreset()
    {
        int prevIndex = currentPresetIndex - 1;
        if (prevIndex < 0) prevIndex = presets.Length - 1;
        
        LoadPreset(prevIndex);
        
        if (presetDropdown != null)
        {
            presetDropdown.value = prevIndex;
        }
    }
    
    /// <summary>
    /// Get currently active preset
    /// </summary>
    public MusicalModePreset GetCurrentPreset()
    {
        if (presets != null && currentPresetIndex >= 0 && currentPresetIndex < presets.Length)
        {
            return presets[currentPresetIndex];
        }
        return null;
    }
}
