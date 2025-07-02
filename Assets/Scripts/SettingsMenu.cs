using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    [Header("UI")]
    public GameObject settingsPanel;
    public GameObject shopPanel;
    public GameObject hangerPanel;
    
    void Start()
    {
        LoadSettings();
    }
    
    public void OnMusicVolumeChanged()
    {
        float volume = musicVolumeSlider.value;
        AudioManager.Instance.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    
    public void OnSFXVolumeChanged()
    {
        float volume = sfxVolumeSlider.value;
        AudioManager.Instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    
    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeInHierarchy);
    }

    public void ToggleHanger()
    {
        hangerPanel.SetActive(!hangerPanel.activeInHierarchy);
    }
    
    void LoadSettings()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
    }
}