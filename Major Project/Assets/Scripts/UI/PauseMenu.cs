// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;

    [SerializeField] GameObject pausedUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pausedUI.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    void PauseGame()
    {
        pausedUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
