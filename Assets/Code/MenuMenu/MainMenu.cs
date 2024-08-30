using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSourceMusic;
    void Start()
    {
        float savedMusic = PlayerPrefs.GetFloat("Music", 1.0f);
        audioSourceMusic.volume = savedMusic;

    }
    public void PlayGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1);//game scene to change  file->build settings
    }
    public void Quit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
