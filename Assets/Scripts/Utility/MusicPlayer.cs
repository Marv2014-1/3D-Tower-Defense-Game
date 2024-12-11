using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip menuSong, storySong, gameSong;
    public static MusicPlayer instance;
    public static AudioSource music;
    public AudioMixer audioMixer;
    public float masterVolume = 1f, musicVolume = 1f, sfxVolume = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    // Allows the music player to listen for scene changes
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        music = this.GetComponent<AudioSource>();
        audioMixer = transform.GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;
    }

    // Calls everytime a scene changes and changes the music track
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "MainMenu")
        {
            music.clip = menuSong;
        }
        else if (sceneName == "Story")
        {
            music.clip = storySong;
        } else if (sceneName == "Game")
        {
            music.clip = gameSong;
        }
        else
        {
            music.clip = null;
        }

        AudioListener.pause = false;
        music.Play();
    }

    public void MasterVolume(float volume)
    {
        masterVolume = volume;
        audioMixer.SetFloat("masterVol", Mathf.Log10(volume) * 20);
    }

    public void MusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20);
    }

    public void SFXVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("sfxVol", Mathf.Log10(volume) * 20); 
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
