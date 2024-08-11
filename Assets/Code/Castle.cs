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
        health -= amount;
        healthBar.SetHealth(health);

        if (health <= 0)
        {
            AldenGenerator.LogAldenChat("The castle was destroyed and now the monsters are going to kill you");
        }
    }

}
