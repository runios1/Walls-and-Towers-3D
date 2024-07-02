using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    public float health = 100;
    public int coins = 0;
    public HealthBar healthBar;
    public CoinCounter coinCounter;
    void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetCoins(int amount)
    {
        coins += amount;
        coinCounter.IncreaseCounter(amount);
    }

    public void LoseCoins(int amount)
    {
        coins -= amount;
        coinCounter.DecreaseCounter(amount);
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    void Die()
    {

    }


}
