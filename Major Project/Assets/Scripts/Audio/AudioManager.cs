using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound currentMusic = null;
    public AudioMixerGroup mainMixer;
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
            s.source.outputAudioMixerGroup = mainMixer;

            if (s.name == "MusicMainMenuIntro")
                currentMusic = s;
            
        }

        
    }

    private void Update() {
        ScenePlayMusic();
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
    public void PlayMusic(string loopName, string introName){
        Sound loopS = null, introS = null;

        foreach(Sound s in sounds){ //checks the array for matching name
            if (s.name == loopName){
                loopS = s;
            } else if (s.name == introName){
                introS = s;
            }
        }

        if (currentMusic != null){
            currentMusic.source.Stop();
            Debug.Log("MUSIC STOP: " + currentMusic.name);
        }
        
        if(introS == null){
            currentMusic = loopS;
            currentMusic.source.Play();
        } else {
            currentMusic = introS;
            currentMusic.source.Play();
            currentMusic = loopS;
            currentMusic.source.PlayDelayed(introS.clip.length);
        }

        currentMusic = loopS;
        currentMusic.source.Play();
    }

    void ScenePlayMusic(){
        sTo = SceneManager.GetActiveScene();
        if(sTo != sFrom){
            if(sTo.name == "Main menu"){
                PlayMusic("MusicMainMenuLoop", "MusicMainMenuIntro");
                Debug.Log("Playing main menu");
            }    
            else if (sTo.name == "Prototype"){
                PlayMusic("MusicLevel1Loop", null);
                Debug.Log("Playing other");
            }
            sFrom = sTo;
        }
    }
}