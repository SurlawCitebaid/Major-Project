// Juliet Gobran
// Script for main menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // loads game scene
   public void PlayGame ()
   {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

    // exists application
   public void Exitgame ()
   {
        Debug.Log ("EXIT!");
        Application.Quit();
   }
}
