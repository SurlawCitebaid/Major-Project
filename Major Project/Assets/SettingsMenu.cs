// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
  
    public AudioMixer aM;
    public void SetVolume (float vol)
    {
        aM.SetFloat("MasterVolume", vol);
    }

    ToggleGroup toggleGroup;
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void SetGraphics()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        int qualityIndex = 2;

        if (toggle.name == "Toggle Low")
        {
            qualityIndex = 0;
        } 
        else if (toggle.name == "Toggle Med")
        {
            qualityIndex = 1;
        } 
        else if (toggle.name == "Toggle High")
        {
            qualityIndex = 2;
        }
        QualitySettings.SetQualityLevel(qualityIndex);

        Debug.Log("Updated Quality to index " + toggle.name);
    }



}
