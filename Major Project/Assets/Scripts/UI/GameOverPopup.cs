// Juliet Gobran
// Script for Game Over menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPopup : MonoBehaviour
{
    public TMP_Text timeValue; // access text UI
    public GameObject gameOverScreen;

    // Displays final play time
    void Update() 
    {
        if(PlayerController.Instance.dead)
        {
            gameOverScreen.SetActive(true);
            timeValue.text = PlayerController.Instance.timeAlive.ToString("0.00");
        }
        
    }

    // Loads game again
    public void Restart()
    {
        Time.timeScale = 1f;
        FindObjectOfType<Inventory>().DestroyOnMainMenu();
        SceneManager.LoadScene(1);
    }

    // Back to main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<Inventory>().DestroyOnMainMenu();
        SceneManager.LoadScene(0);
    }
}
