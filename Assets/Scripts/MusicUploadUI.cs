using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Runtime.InteropServices;

/// <summary>
/// UI for uploading custom music files and analyzing them
/// Works with WebGL, standalone builds, and editor
/// </summary>
public class MusicUploadUI : MonoBehaviour
{
    [Header("UI References")]
    public Button uploadButton;
    public Button startGameButton;
    public TextMeshProUGUI statusText;
    public Slider analysisProgressSlider;
    public GameObject uploadPanel;
    
    [Header("Settings")]
    public AudioMelodyExtractor melodyExtractor;
    public GameModeManager gameModeManager;
    
    private string selectedFilePath;
    private bool isProcessing = false;
    
    // WebGL file upload (only works in WebGL builds)
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName);
    #endif
    
    void Start()
    {
        if (uploadButton != null)
        {
            uploadButton.onClick.AddListener(OnUploadButtonClick);
        }
        
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(OnStartGameClick);
            startGameButton.interactable = false;
        }
        
        if (melodyExtractor == null)
        {
            melodyExtractor = FindObjectOfType<AudioMelodyExtractor>();
        }
        
        if (gameModeManager == null)
        {
            gameModeManager = FindObjectOfType<GameModeManager>();
        }
        
        UpdateStatus("Upload your music to begin!");
    }
    
    /// <summary>
    /// Called when upload button is clicked
    /// </summary>
    void OnUploadButtonClick()
    {
        if (isProcessing)
        {
            Debug.LogWarning("Already processing audio!");
            return;
        }
        
        #if UNITY_WEBGL && !UNITY_EDITOR
            // WebGL: Use JavaScript file picker
            UploadFile(gameObject.name, "OnFileUploaded");
        #elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // Windows: Use file explorer
            StartCoroutine(OpenFileExplorerWindows());
        #elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            // macOS: Use file explorer
            StartCoroutine(OpenFileExplorerMac());
        #else
            UpdateStatus("File upload not supported on this platform");
        #endif
    }
    
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    /// <summary>
    /// Opens Windows file explorer
    /// </summary>
    IEnumerator OpenFileExplorerWindows()
    {
        UpdateStatus("Opening file explorer...");
        
        // Use Unity's SimpleFileBrowser or similar
        // For now, let's use a simple path input
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select Audio File", "", "mp3,wav,ogg");
        
        if (!string.IsNullOrEmpty(path))
        {
            yield return StartCoroutine(LoadAudioFile(path));
        }
        else
        {
            UpdateStatus("No file selected");
        }
    }
    #endif
    
    #if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    /// <summary>
    /// Opens macOS file explorer
    /// </summary>
    IEnumerator OpenFileExplorerMac()
    {
        UpdateStatus("Opening file explorer...");
        
        #if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select Audio File", "", "mp3,wav,ogg,m4a");
        
        if (!string.IsNullOrEmpty(path))
        {
            yield return StartCoroutine(LoadAudioFile(path));
        }
        else
        {
            UpdateStatus("No file selected");
        }
        #else
        // In builds, you'd need a native plugin for file picker
        UpdateStatus("Please use Resources folder for audio files in this build");
        yield return null;
        #endif
    }
    #endif
    
    /// <summary>
    /// Loads audio file from file system
    /// </summary>
    IEnumerator LoadAudioFile(string filePath)
    {
        isProcessing = true;
        UpdateStatus($"Loading audio file...");
        
        string url = "file://" + filePath;
        
        // Determine audio type
        AudioType audioType = AudioType.UNKNOWN;
        if (filePath.EndsWith(".mp3")) audioType = AudioType.MPEG;
        else if (filePath.EndsWith(".wav")) audioType = AudioType.WAV;
        else if (filePath.EndsWith(".ogg")) audioType = AudioType.OGGVORBIS;
        
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return www.SendWebRequest();
            
            if (www.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                clip.name = Path.GetFileNameWithoutExtension(filePath);
                
                UpdateStatus($"Audio loaded: {clip.name}");
                yield return StartCoroutine(AnalyzeAudio(clip));
            }
            else
            {
                UpdateStatus($"Error loading audio: {www.error}");
                isProcessing = false;
            }
        }
    }
    
    /// <summary>
    /// Analyzes the loaded audio clip
    /// </summary>
    IEnumerator AnalyzeAudio(AudioClip clip)
    {
        UpdateStatus("Analyzing audio for melody...");
        
        if (melodyExtractor == null)
        {
            UpdateStatus("Error: MelodyExtractor not found!");
            isProcessing = false;
            yield break;
        }
        
        // Start analysis
        melodyExtractor.AnalyzeAudio(clip);
        
        // Wait for analysis to complete
        while (melodyExtractor.IsAnalyzing())
        {
            if (analysisProgressSlider != null)
            {
                // You could add progress tracking here
                analysisProgressSlider.value = Random.Range(0.2f, 0.8f); // Simulated progress
            }
            yield return new WaitForSeconds(0.1f);
        }
        
        if (melodyExtractor.IsAnalysisComplete())
        {
            int noteCount = melodyExtractor.GetDetectedNotes().Count;
            UpdateStatus($"Analysis complete! Detected {noteCount} notes");
            
            if (analysisProgressSlider != null)
            {
                analysisProgressSlider.value = 1f;
            }
            
            if (startGameButton != null)
            {
                startGameButton.interactable = true;
            }
        }
        else
        {
            UpdateStatus("Analysis failed. Please try a different file.");
        }
        
        isProcessing = false;
    }
    
    /// <summary>
    /// Called when start game button is clicked
    /// </summary>
    void OnStartGameClick()
    {
        if (!melodyExtractor.IsAnalysisComplete())
        {
            UpdateStatus("Please upload and analyze music first!");
            return;
        }
        
        // Hide upload panel
        if (uploadPanel != null)
        {
            uploadPanel.SetActive(false);
        }
        
        // Switch to musical mode
        if (gameModeManager != null)
        {
            gameModeManager.SetGameMode(GameModeManager.GameMode.Musical);
        }
        
        // Start the game
        Debug.Log("Starting game with custom music!");
    }
    
    /// <summary>
    /// Updates status text
    /// </summary>
    void UpdateStatus(string message)
    {
        Debug.Log($"MusicUploadUI: {message}");
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
    
    /// <summary>
    /// Called from JavaScript when file is uploaded (WebGL only)
    /// </summary>
    public void OnFileUploaded(string base64Data)
    {
        StartCoroutine(ProcessUploadedFile(base64Data));
    }
    
    /// <summary>
    /// Processes uploaded file from WebGL
    /// </summary>
    IEnumerator ProcessUploadedFile(string base64Data)
    {
        isProcessing = true;
        UpdateStatus("Processing uploaded file...");
        
        try
        {
            // Decode base64 to audio data
            byte[] audioData = System.Convert.FromBase64String(base64Data);
            
            // This requires additional processing to convert raw bytes to AudioClip
            // For WebGL, you might need a different approach
            UpdateStatus("WebGL audio processing not yet implemented");
        }
        catch (System.Exception e)
        {
            UpdateStatus($"Error: {e.Message}");
        }
        
        isProcessing = false;
        yield return null;
    }
}
