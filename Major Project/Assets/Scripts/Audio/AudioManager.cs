using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound currentMusic = null;
    public static AudioManager instance;
    public Scene sTo, sFrom;


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

            if (s.name == "MusicMainMenu")
                currentMusic = s;
            
        }

        
    }

    private void Update() {
        StartPlayMusic();
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
    public void PlayMusic(string name){
        foreach(Sound s in sounds){ //checks the array for matching name
            if (s.name == name){
                
                if (currentMusic != null){
                    currentMusic.source.Stop();
                    Debug.Log("MUSIC STOP: " + currentMusic.name);
                }
                    
                currentMusic = s;
                currentMusic.source.Play();
            }
        } 
    }

    void StartPlayMusic(){
        sTo = SceneManager.GetActiveScene();
        if(sTo != sFrom){
            if(sTo.name == "Main menu"){
                PlayMusic("MusicMainMenu");
                Debug.Log("Playing main menu");
            }    
            else{
                PlayMusic("MusicBossRoom");
                Debug.Log("Playing other");
            }
            sFrom = sTo;
        }
    }
}