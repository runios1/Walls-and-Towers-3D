using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    public float health = 100;
    public int coins = 0;
    public HealthBar healthBar;
    public CoinCounter coinCounter;
    public Shop shop;

    public Animator animator;
    public Transform attackPoint;
    public Transform player;
    public float attackRange = 3f;
    public string enemyTag = "Enemy";

    void Start()
    {
        healthBar.SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsAttacking())
        {
            Attack();
        }

        // Placeables

        if (Input.GetKeyDown(KeyCode.Q))
        {
            shop.Buy("Tower");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            shop.Buy("Wall");
        }

    }

    public void GetCoins(int amount)
    {
        coins += amount;
        coinCounter.IncreaseCounter(amount);
    }

    public bool LoseCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            coinCounter.DecreaseCounter(amount);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins");
            return false;
        }
    }

    public void TakeDamage(float damage)
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
    void Attack()
    {
        animator.SetTrigger("Attack");

        // Transform newAttackPoint = transform.position + attackDirection * attackRange;

        // Detect enemies in range of the attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);

            if (enemy.CompareTag(enemyTag))
            {
            }
        }
    }
    bool IsAttacking()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }

    public void PlaceItem(Placeable item)
    {
        Vector3 adjustedPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Instantiate(item.prefab, adjustedPosition, transform.rotation);
    }
}
