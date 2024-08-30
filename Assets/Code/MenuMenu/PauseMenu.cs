using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;
    public GameObject pauseMenu;
    public AudioSource audioSourceMusic;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Debug.Log("Game is paused");
        AudioSourceIsEnable(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        Debug.Log("Game is resumed");
        AudioSourceIsEnable(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void AudioSourceIsEnable(bool enable)
    {
        // Get all AudioSource components in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (enable)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Pause();
            }
        }
        audioSourceMusic.Play();
    }




    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);//game scene to change  file->build settings

    }
    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
