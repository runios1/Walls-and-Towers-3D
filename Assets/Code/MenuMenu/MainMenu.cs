using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);//game scene to change  file->build settings
    }
    public void Quit()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}