using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public float weightEnemiesKilled = 1.0f;
    public float weightCastleHealth = 2.0f;
    public float weightCoinsUsed = -0.5f; // Negative weight for coins used
    public float weightCoinsGained = 0.5f; // Positive weight for coins gained

    private int enemiesKilled = 0;
    private int coinsGained = 0;
    private int coinsUsed = 0;

    public Castle castle; // Reference to the Castle script
    public PlayerMainScript player; // Reference to the Player script


    private float initialCastleHealth;

    void Update()
    {
        // Calculate and display the current score for debugging or UI purposes
        float score = CalculateScore();
        castle = FindObjectOfType<Castle>();
        initialCastleHealth = castle.health;
        player = FindObjectOfType<PlayerMainScript>();

        Debug.Log("Current Score: " + score);
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }

    public void CoinsGained(int amount)
    {
        coinsGained += amount;
    }

    public void CoinsUsed(int amount)
    {
        coinsUsed += amount;
    }

    private float CalculateScore()
    {
        float normalizedCastleHealth = castle.health / 100f; // Assuming castle health is out of 100
        int netCoins = coinsGained - coinsUsed; // Net coins calculation
        return (weightEnemiesKilled * enemiesKilled) +
               (weightCastleHealth * normalizedCastleHealth) +
               (weightCoinsUsed * coinsUsed) + 
               (weightCoinsGained * netCoins);
    }
}

