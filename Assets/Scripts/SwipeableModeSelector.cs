using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Swipeable mode selector for main menu
/// Allows users to swipe left/right to switch between game modes
/// Each mode panel shows its high score and can be played by clicking the player sprite
/// </summary>
public class SwipeableModeSelector : MonoBehaviour
{
    [Header("Mode Panels")]
    [Tooltip("Panel/GameObject for Normal Mode")]
    public GameObject normalModePanel;
    
    [Tooltip("Panel/GameObject for Musical Mode")]
    public GameObject musicalModePanel;
    
    [Tooltip("Panel/GameObject for Upload Music Mode")]
    public GameObject uploadModePanel;
    
    [Header("UI References for Each Mode")]
    [Tooltip("Text showing mode name (e.g., 'Mode: Normal')")]
    public TextMeshProUGUI normalModeText;
    public TextMeshProUGUI musicalModeText;
    public TextMeshProUGUI uploadModeText;
    
    [Tooltip("Text showing high score for each mode")]
    public TextMeshProUGUI normalHighScoreText;
    public TextMeshProUGUI musicalHighScoreText;
    public TextMeshProUGUI uploadHighScoreText;
    
    [Tooltip("Player sprite buttons to start each mode")]
    public Button normalPlayButton;
    public Button musicalPlayButton;
    public Button uploadPlayButton;
    
    [Header("Scene Names")]
    public string normalModeScene = "Game";
    public string musicalModeScene = "MusicMode";
    public string uploadMusicScene = "PlayOwn";
    
    [Header("Swipe Settings")]
    [Tooltip("Minimum swipe distance to trigger mode change (in pixels)")]
    public float minSwipeDistance = 50f;
    
    [Tooltip("Maximum time for a swipe (seconds)")]
    public float maxSwipeTime = 1f;
    
    [Tooltip("Smooth transition duration between modes")]
    public float transitionDuration = 0.3f;
    
    [Header("Visual Settings")]
    [Tooltip("Scale of inactive mode panels")]
    public float inactiveScale = 0.8f;
    
    [Tooltip("Alpha of inactive mode panels")]
    public float inactiveAlpha = 0.5f;
    
    [Header("Optional Indicators")]
    [Tooltip("Mode indicators component (optional - for dots/arrows)")]
    public ModeIndicators modeIndicators;
    
    // Current mode state
    private int currentModeIndex = 0; // 0 = Normal, 1 = Musical, 2 = Upload
    private GameObject[] modePanels;
    
    // Swipe detection
    private Vector2 swipeStartPos;
    private float swipeStartTime;
    private bool isSwiping = false;
    
    // Transition
    private bool isTransitioning = false;
    
    void Start()
    {
        // Initialize mode panels array
        modePanels = new GameObject[] { normalModePanel, musicalModePanel, uploadModePanel };
        
        // Ensure HighScoreManager exists
        if (HighScoreManager.Instance == null)
        {
            GameObject highScoreManager = new GameObject("HighScoreManager");
            highScoreManager.AddComponent<HighScoreManager>();
        }
        
        // Setup button listeners
        if (normalPlayButton != null)
            normalPlayButton.onClick.AddListener(() => PlayMode(0));
        
        if (musicalPlayButton != null)
            musicalPlayButton.onClick.AddListener(() => PlayMode(1));
        
        if (uploadPlayButton != null)
            uploadPlayButton.onClick.AddListener(() => PlayMode(2));
        
        // Initialize UI
        UpdateModeTexts();
        UpdateHighScores();
        ShowCurrentMode();
        
        Debug.Log("Swipeable Mode Selector initialized. Swipe left/right to change modes!");
    }
    
    void Update()
    {
        if (isTransitioning)
            return;
        
        HandleSwipeInput();
    }
    
    /// <summary>
    /// Handle touch/mouse swipe input
    /// </summary>
    void HandleSwipeInput()
    {
        // Mouse/Touch input
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartPos = Input.mousePosition;
            swipeStartTime = Time.time;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 swipeEndPos = Input.mousePosition;
            float swipeTime = Time.time - swipeStartTime;
            
            ProcessSwipe(swipeStartPos, swipeEndPos, swipeTime);
            isSwiping = false;
        }
        
        // Keyboard input for testing
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwipeRight(); // Swipe right shows previous mode (left arrow = go left)
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwipeLeft(); // Swipe left shows next mode (right arrow = go right)
        }
    }
    
    /// <summary>
    /// Process swipe gesture
    /// </summary>
    void ProcessSwipe(Vector2 startPos, Vector2 endPos, float swipeTime)
    {
        // Check if swipe was fast enough
        if (swipeTime > maxSwipeTime)
            return;
        
        Vector2 swipeVector = endPos - startPos;
        float swipeDistance = swipeVector.magnitude;
        
        // Check if swipe was long enough
        if (swipeDistance < minSwipeDistance)
            return;
        
        // Determine swipe direction (horizontal only)
        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.x > 0)
            {
                // Swipe right - show previous mode
                SwipeRight();
            }
            else
            {
                // Swipe left - show next mode
                SwipeLeft();
            }
        }
    }
    
    /// <summary>
    /// Swipe left to next mode
    /// </summary>
    void SwipeLeft()
    {
        if (currentModeIndex < modePanels.Length - 1)
        {
            currentModeIndex++;
            StartCoroutine(TransitionToMode(currentModeIndex));
            
            // Update indicators
            if (modeIndicators != null)
            {
                modeIndicators.UpdateIndicators(currentModeIndex);
            }
            
            Debug.Log($"Swiped to mode: {GetModeName(currentModeIndex)}");
        }
    }
    
    /// <summary>
    /// Swipe right to previous mode
    /// </summary>
    void SwipeRight()
    {
        if (currentModeIndex > 0)
        {
            currentModeIndex--;
            StartCoroutine(TransitionToMode(currentModeIndex));
            
            // Update indicators
            if (modeIndicators != null)
            {
                modeIndicators.UpdateIndicators(currentModeIndex);
            }
            
            Debug.Log($"Swiped to mode: {GetModeName(currentModeIndex)}");
        }
    }
    
    /// <summary>
    /// Show current mode panel immediately
    /// </summary>
    void ShowCurrentMode()
    {
        for (int i = 0; i < modePanels.Length; i++)
        {
            if (modePanels[i] != null)
            {
                bool isActive = i == currentModeIndex;
                modePanels[i].SetActive(isActive);
                
                // Set scale and alpha
                if (isActive)
                {
                    modePanels[i].transform.localScale = Vector3.one;
                    SetPanelAlpha(modePanels[i], 1f);
                }
                else
                {
                    modePanels[i].transform.localScale = Vector3.one * inactiveScale;
                    SetPanelAlpha(modePanels[i], inactiveAlpha);
                }
            }
        }
    }
    
    /// <summary>
    /// Smooth transition to target mode
    /// </summary>
    IEnumerator TransitionToMode(int targetIndex)
    {
        isTransitioning = true;
        
        // Enable target panel
        if (modePanels[targetIndex] != null)
        {
            modePanels[targetIndex].SetActive(true);
        }
        
        float elapsed = 0f;
        
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            
            // Smooth step for better feel
            t = Mathf.SmoothStep(0f, 1f, t);
            
            // Update all panels
            for (int i = 0; i < modePanels.Length; i++)
            {
                if (modePanels[i] != null && modePanels[i].activeSelf)
                {
                    bool isTarget = i == targetIndex;
                    
                    // Scale
                    float targetScale = isTarget ? 1f : inactiveScale;
                    float currentScale = modePanels[i].transform.localScale.x;
                    float newScale = Mathf.Lerp(currentScale, targetScale, t);
                    modePanels[i].transform.localScale = Vector3.one * newScale;
                    
                    // Alpha
                    float targetAlpha = isTarget ? 1f : inactiveAlpha;
                    SetPanelAlpha(modePanels[i], Mathf.Lerp(inactiveAlpha, targetAlpha, t));
                }
            }
            
            yield return null;
        }
        
        // Disable inactive panels
        for (int i = 0; i < modePanels.Length; i++)
        {
            if (i != targetIndex && modePanels[i] != null)
            {
                modePanels[i].SetActive(false);
            }
        }
        
        isTransitioning = false;
    }
    
    /// <summary>
    /// Set alpha for all CanvasGroup components in panel
    /// </summary>
    void SetPanelAlpha(GameObject panel, float alpha)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = alpha;
    }
    
    /// <summary>
    /// Update mode text labels
    /// </summary>
    void UpdateModeTexts()
    {
        if (normalModeText != null)
            normalModeText.text = "Mode: Normal";
        
        if (musicalModeText != null)
            musicalModeText.text = "Mode: Musical";
        
        if (uploadModeText != null)
            uploadModeText.text = "Mode: Upload";
    }
    
    /// <summary>
    /// Update high score displays
    /// </summary>
    void UpdateHighScores()
    {
        if (HighScoreManager.Instance == null)
            return;
        
        int normalScore = HighScoreManager.Instance.GetHighScore(HighScoreManager.GameMode.Normal);
        int musicalScore = HighScoreManager.Instance.GetHighScore(HighScoreManager.GameMode.Musical);
        int uploadScore = HighScoreManager.Instance.GetHighScore(HighScoreManager.GameMode.Upload);
        
        if (normalHighScoreText != null)
            normalHighScoreText.text = $"High Score: {normalScore}";
        
        if (musicalHighScoreText != null)
            musicalHighScoreText.text = $"High Score: {musicalScore}";
        
        if (uploadHighScoreText != null)
            uploadHighScoreText.text = $"High Score: {uploadScore}";
    }
    
    /// <summary>
    /// Get mode name by index
    /// </summary>
    string GetModeName(int index)
    {
        switch (index)
        {
            case 0: return "Normal";
            case 1: return "Musical";
            case 2: return "Upload";
            default: return "Unknown";
        }
    }
    
    /// <summary>
    /// Play selected mode
    /// </summary>
    void PlayMode(int modeIndex)
    {
        string sceneName = "";
        
        switch (modeIndex)
        {
            case 0:
                sceneName = normalModeScene;
                Debug.Log("Starting Normal Mode...");
                break;
            case 1:
                sceneName = musicalModeScene;
                Debug.Log("Starting Musical Mode...");
                break;
            case 2:
                sceneName = uploadMusicScene;
                Debug.Log("Starting Upload Music Mode...");
                break;
        }
        
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Use SceneTransitionManager if available
            if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
    
    /// <summary>
    /// Public method to quit game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
