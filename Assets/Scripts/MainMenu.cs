using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private string gameSceneName = "Game";
    
    private void Start()
    {
        // Make sure HighScoreManager exists
        if (HighScoreManager.Instance == null)
        {
            GameObject highScoreManager = new GameObject("HighScoreManager");
            highScoreManager.AddComponent<HighScoreManager>();
        }
        
        UpdateHighScoreText();
    }
    
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + HighScoreManager.Instance.HighScore;
        }
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}