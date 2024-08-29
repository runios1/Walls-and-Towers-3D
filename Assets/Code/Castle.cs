using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
// using static AldenGenerator;

public class Castle : MonoBehaviour
{
    public float health = 100;
    public HealthBar healthBar;
    public GameObject GameOverMenu;
    public AudioClip gameoverClip;
    void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    public void TakeDamage(float amount)
    {
        if (health <= 0) return; // FIXME: Placeholder to not keep executing this function even though the castle is destroyed until gameover is implemented
        health -= amount;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameOverMenu.SetActive(true);
            GameOverMenu.GetComponentInChildren<GameOverMenu>().LoadMenu();
            AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];

            foreach (AudioSource audioS in allAudioSources)
            {
                audioS.Stop();
            }

            GameOverMenu.GetComponent<AudioSource>().Play();

            // await LogAldenChat("The castle was destroyed and now the monsters are going to kill you");
        }
    }
}
