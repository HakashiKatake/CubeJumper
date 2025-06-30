using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIhandler : MonoBehaviour 
{
    public void ReplayGame()
    {
        // Clear any persistent data if needed
        Time.timeScale = 1f; // Reset time scale in case it was paused
        
        // Reload the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}