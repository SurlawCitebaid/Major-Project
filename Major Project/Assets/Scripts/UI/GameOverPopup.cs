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

    // Displays final play time
    void Update() 
    {
        timeValue.text = Time.time.ToString("0.00");
    }

    // Loads game again
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    // Back to main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
