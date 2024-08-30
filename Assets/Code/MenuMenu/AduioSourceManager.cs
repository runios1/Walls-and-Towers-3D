using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class AduioSourceManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider musicSlider;
    public AudioSource audioSourceMusic;

    // Start is called before the first frame update
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        float savedMusic = PlayerPrefs.GetFloat("Music", 1.0f);
        volumeSlider.value = savedVolume;
        musicSlider.value = savedMusic;
        audioSourceMusic.volume = savedMusic;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicSlider.onValueChanged.AddListener(SetMusic);
    }


    private void SetVolume(float volume)
    {
        Debug.Log($"volume change to {volume}");

        PlayerPrefs.SetFloat("Volume", volume);

    }
    private void SetMusic(float volume)
    {
        Debug.Log($"music change to {volume}");
        audioSourceMusic.volume = volume;
        PlayerPrefs.SetFloat("Music", volume);

    }
}
