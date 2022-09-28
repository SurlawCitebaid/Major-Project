using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    Sound currentMusic = null;
    Sound currentIntro = null;
    public AudioMixerGroup mainMixer;
    public static AudioManager instance;
    Scene sTo, sFrom;


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

            if (s.name == "MusicMainMenuLoop")
                currentMusic = s;

            if (s.name == "MusicMainMenuIntro")
                currentIntro = s;
            
            
        }

        
    }

    private void Update() {
        ScenePlayMusic(false);
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
            //StartCoroutine(FadeOut(false, 0.01f, 1f));
            currentMusic.source.Stop();
        }
            
        if (currentIntro != null){
            //StartCoroutine(FadeOut(true, 0.01f, 1f));
            currentIntro.source. Stop();
        }

            //StartCoroutine(FadeIn(false, 0.01f, 1f));
            //StartCoroutine(FadeIn(true, 0.01f, 1f));

        if(introS == null){ //no intro track to the sound
            currentMusic = loopS;
            currentMusic.source.Play();
        } else {
            currentIntro = introS;
            currentIntro.source.Play();
            currentMusic = loopS;
            currentMusic.source.PlayDelayed(introS.clip.length);
        }

    }

    public void ScenePlayMusic(bool bossKilled){
        sTo = SceneManager.GetActiveScene();
        if(sTo != sFrom || bossKilled){
            switch (sTo.name)
            {
                case "Main menu":
                    PlayMusic("MusicMainMenuLoop", "MusicMainMenuIntro");
                break;

                case "Prototype":
                    PlayMusic("MusicLevel1Loop", null);
                break;

                case "Prototype 1":
                    PlayMusic("MusicLevel2Loop", null);
                break;

                default:
                break;
            }
            
            sFrom = sTo;
        }
    }

    IEnumerator FadeIn(bool musicType, float speed, float timeToFade) { //musicType: false for currentMusic, true for currentIntro
        Debug.Log("Call FadeIn");

        currentMusic.source.volume = 0;
        currentIntro.source.volume = 0;

        for (float i = 0; i <= 1f; i += speed){
            if(musicType)
                currentMusic.source.volume += speed;
            else
                currentIntro.source.volume += speed;

            yield return new WaitForSeconds(speed/timeToFade);
        }
    }

    IEnumerator FadeOut(bool musicType, float speed, float timeToFade) { //musicType: false for currentMusic, true for currentIntro
        Debug.Log("Call FadeOut");
        for (float i = 1f; i > 0; i -= speed){
            if(musicType)
                currentMusic.source.volume -= speed;
            else
                currentIntro.source.volume -= speed;

            yield return new WaitForSeconds(timeToFade/speed);
        }
    }
}