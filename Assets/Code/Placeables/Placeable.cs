using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Placeable : MonoBehaviour
{
    [Header("Info")]
    public GameObject prefab;
    public string placeableName;
    public int cost;
    public int sellValue;
    public float health;
    [TextArea(1, 2)]
    public string description;
    public void TakeDamage(float damage)
    {
        health -= damage;
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