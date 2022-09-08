using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake() {
        
        //Only one will exist
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);// will continue throiugh scenes

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    //call the method in the gameObject, findObjectOfType
    public void Play(string name){
        foreach(Sound s in sounds){ //checks the array for matching name
            if (s.name == name){
                s.source.Play();
                break;
            }
        } 
    }
}
