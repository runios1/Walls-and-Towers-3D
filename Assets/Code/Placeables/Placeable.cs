using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class Placeable : MonoBehaviour
{
    [Header("Info")]
    public GameObject prefab;
    public string placeableName;
    public int cost;
    public int sellValue;
    public float health;
    public HealthBar healthBar;
    [TextArea(1, 2)]
    public string description;
    public virtual void Start()
    {
        healthBar.SetMaxHealth(health);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}