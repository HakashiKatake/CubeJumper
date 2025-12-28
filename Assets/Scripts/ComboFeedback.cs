using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Shows floating text feedback for combo breaks and perfect jumps
/// Optional visual enhancement for combo system
/// </summary>
public class ComboFeedback : MonoBehaviour
{
    [Header("Prefab")]
    [Tooltip("TextMeshPro prefab for floating text")]
    public GameObject floatingTextPrefab;
    
    [Header("Messages")]
    public string perfectJumpMessage = "PERFECT!";
    public string comboBreakMessage = "COMBO BREAK!";
    public string firstPerfectMessage = "NICE!";
    
    [Header("Colors")]
    public Color perfectColor = Color.green;
    public Color comboBreakColor = Color.red;
    
    [Header("Animation")]
    public float floatSpeed = 2f;
    public float fadeSpeed = 1f;
    public float duration = 1.5f;
    
    public static ComboFeedback Instance { get; private set; }
    
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
    
    /// <summary>
    /// Show feedback for perfect jump
    /// </summary>
    public void ShowPerfectJump(Vector3 position, int comboCount)
    {
        string message = comboCount == 1 ? firstPerfectMessage : perfectJumpMessage;
        ShowFloatingText(message, position, perfectColor);
    }
    
    /// <summary>
    /// Show feedback for combo break
    /// </summary>
    public void ShowComboBreak(Vector3 position, int lostCombo)
    {
        string message = $"{comboBreakMessage}\n-{lostCombo}";
        ShowFloatingText(message, position, comboBreakColor);
    }
    
    /// <summary>
    /// Show floating text at position
    /// </summary>
    void ShowFloatingText(string text, Vector3 position, Color color)
    {
        if (floatingTextPrefab == null)
            return;
            
        GameObject floatingText = Instantiate(floatingTextPrefab, position, Quaternion.identity);
        TextMeshPro tmp = floatingText.GetComponent<TextMeshPro>();
        
        if (tmp != null)
        {
            tmp.text = text;
            tmp.color = color;
            StartCoroutine(AnimateFloatingText(floatingText, tmp));
        }
        else
        {
            Destroy(floatingText, duration);
        }
    }
    
    /// <summary>
    /// Animate floating text
    /// </summary>
    IEnumerator AnimateFloatingText(GameObject textObj, TextMeshPro tmp)
    {
        Vector3 startPos = textObj.transform.position;
        Color startColor = tmp.color;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Float upward
            textObj.transform.position = startPos + Vector3.up * (floatSpeed * elapsed);
            
            // Fade out
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            tmp.color = newColor;
            
            yield return null;
        }
        
        Destroy(textObj);
    }
}
