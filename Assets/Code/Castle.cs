using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public float health = 100;
    public HealthBar healthBar;

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
            AldenGenerator.LogAldenChat("The castle was destroyed and now the monsters are going to kill you");
        }
    }
}
