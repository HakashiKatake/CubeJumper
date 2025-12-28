using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visual indicators for swipeable mode selector
/// Shows dots or arrows to indicate which mode is active and swipe direction
/// </summary>
public class ModeIndicators : MonoBehaviour
{
    [Header("Dot Indicators (Optional)")]
    [Tooltip("Array of 3 dot images (0=Normal, 1=Musical, 2=Upload)")]
    public Image[] dotIndicators;
    
    [Tooltip("Color for active dot")]
    public Color activeDotColor = Color.white;
    
    [Tooltip("Color for inactive dots")]
    public Color inactiveDotColor = new Color(1f, 1f, 1f, 0.3f);
    
    [Header("Arrow Indicators (Optional)")]
    [Tooltip("Left arrow (appears when can swipe left)")]
    public GameObject leftArrow;
    
    [Tooltip("Right arrow (appears when can swipe right)")]
    public GameObject rightArrow;
    
    [Header("Animation")]
    [Tooltip("Pulse animation for active dot")]
    public bool animateActiveDot = true;
    
    [Tooltip("Scale range for pulse (min, max)")]
    public Vector2 pulseScale = new Vector2(1f, 1.2f);
    
    [Tooltip("Pulse speed")]
    public float pulseSpeed = 2f;
    
    private int currentModeIndex = 0;
    private Vector3[] originalDotScales;
    
    void Start()
    {
        // Store original scales
        if (dotIndicators != null && dotIndicators.Length > 0)
        {
            originalDotScales = new Vector3[dotIndicators.Length];
            for (int i = 0; i < dotIndicators.Length; i++)
            {
                if (dotIndicators[i] != null)
                {
                    originalDotScales[i] = dotIndicators[i].transform.localScale;
                }
            }
        }
        
        UpdateIndicators(0); // Start with Normal mode
    }
    
    void Update()
    {
        // Animate active dot
        if (animateActiveDot && dotIndicators != null && currentModeIndex < dotIndicators.Length)
        {
            if (dotIndicators[currentModeIndex] != null)
            {
                float scale = Mathf.Lerp(pulseScale.x, pulseScale.y, 
                    (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
                dotIndicators[currentModeIndex].transform.localScale = originalDotScales[currentModeIndex] * scale;
            }
        }
    }
    
    /// <summary>
    /// Update indicators based on current mode
    /// Call this when mode changes
    /// </summary>
    public void UpdateIndicators(int modeIndex)
    {
        currentModeIndex = modeIndex;
        
        UpdateDots();
        UpdateArrows();
    }
    
    /// <summary>
    /// Update dot indicators
    /// </summary>
    void UpdateDots()
    {
        if (dotIndicators == null || dotIndicators.Length == 0)
            return;
        
        for (int i = 0; i < dotIndicators.Length; i++)
        {
            if (dotIndicators[i] != null)
            {
                // Set color
                dotIndicators[i].color = i == currentModeIndex ? activeDotColor : inactiveDotColor;
                
                // Reset scale for inactive dots
                if (i != currentModeIndex)
                {
                    dotIndicators[i].transform.localScale = originalDotScales[i];
                }
            }
        }
    }
    
    /// <summary>
    /// Update arrow indicators
    /// </summary>
    void UpdateArrows()
    {
        // Left arrow: show if not on first mode (can swipe left)
        if (leftArrow != null)
        {
            leftArrow.SetActive(currentModeIndex > 0);
        }
        
        // Right arrow: show if not on last mode (can swipe right)
        if (rightArrow != null)
        {
            rightArrow.SetActive(currentModeIndex < 2); // 2 is last index (Upload)
        }
    }
}
