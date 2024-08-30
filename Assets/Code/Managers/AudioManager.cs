using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource MusicAudioSource; 
    void Start()
    {
        MusicAudioSource = GetComponent<AudioSource>();

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        float savedMusic = PlayerPrefs.GetFloat("Music", 1.0f);

        // Get all AudioSource components in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = savedVolume;
        }
        MusicAudioSource.volume = savedMusic;

    }

   
}
