using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";
    
    public static HighScoreManager Instance { get; private set; }
    
    private int highScore = 0;
    
    public int HighScore
    {
        get { return highScore; }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }
    
    public bool CheckAndUpdateHighScore(int currentScore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            return true;
        }
        return false;
    }
    
    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        PlayerPrefs.Save();
    }
}