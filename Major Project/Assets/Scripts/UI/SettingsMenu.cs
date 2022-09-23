// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    // Sets volume as slider moves
    public AudioMixer aM;

    private void start()
    {
        float currentLevel;
        aM.GetFloat("MasterVolume", out currentLevel);
        SetVolume(currentLevel);
    }

    public void SetVolume (float vol)
    {
        aM.SetFloat("MasterVolume", vol);
    }
}
