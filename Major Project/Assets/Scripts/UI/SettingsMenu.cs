// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    // Sets volume as slider moves
    public AudioMixer aM;
    public List<ResolutionItem> resolutions = new List<ResolutionItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;

    void Start()
    {
        float currentLevel;
        aM.GetFloat("MasterVolume", out currentLevel);
        SetVolume(currentLevel);
        Screen.SetResolution(1920, 1080, false);
    }

    public void SetVolume (float vol)
    {
        aM.SetFloat("MasterVolume", vol);
    }

    public void ResolutionLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResolutionRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void UpdateOnOk()
    {
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, false);
    }
}

[System.Serializable]
public class ResolutionItem
{
    public int horizontal;
    public int vertical;
}