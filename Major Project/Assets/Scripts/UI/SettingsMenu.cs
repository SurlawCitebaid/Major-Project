// Juliet Gobran
// Script for Settings Menu UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer aM;
    public List<ResolutionItem> resolutions = new();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    public Toggle fullScreenToggle;

    void Start()
    {
        // Get current volume & reflect in slider
        float currentLevel;
        aM.GetFloat("MasterVolume", out currentLevel);
        SetVolume(currentLevel);
        // Sets resolution as most common
        Screen.SetResolution(1920, 1080, true);
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    // Sets volume 
    public void SetVolume (float vol)
    {
        aM.SetFloat("MasterVolume", Mathf.Log10(vol) * 20);
    }

    // Resolution left arrow
    public void ResolutionLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    // Resolution right arrow
    public void ResolutionRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    // Changes label using array
    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    // Screen resolution is updates on OK
    public void UpdateOnOk()
    {
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullScreenToggle.isOn);
    }
}

// Class to hold resolution information
[System.Serializable]
public class ResolutionItem
{
    public int horizontal;
    public int vertical;
}