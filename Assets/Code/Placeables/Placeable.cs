using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    public enum PlaceableState
    {
        Placed,
        Unplaced
    }

    [Header("Info")]
    public GameObject prefab;
    public string placeableName;
    public int cost;
    public int sellValue;
    public float health;
    public HealthBar healthBar;
    [TextArea(1, 2)]
    public string description;

    public PlaceableState state;
    public virtual void Awake()
    {
        healthBar.SetMaxHealth(health);
        state = PlaceableState.Unplaced;
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