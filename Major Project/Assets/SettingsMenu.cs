// Juliet Gobran

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
  
    public AudioMixer aM;
    public void SetVolume (float vol)
    {
        aM.SetFloat("MasterVolume", vol);
    }

}
