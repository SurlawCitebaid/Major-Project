using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //plays sound for objects that doesnt have other scripts
    public void PlaySound(string name){
        FindObjectOfType<AudioManager>().Play(name);
    }
}
