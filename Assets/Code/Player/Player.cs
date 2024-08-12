using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    public float health;
    public int coins;
    public HealthBar healthBar;
    public CoinCounter coinCounter;
    public Shop shop;

    public Animator animator;
    public Transform attackPoint;
    public Transform player;
    public float attackRange = 20f;
    public string enemyTag = "Enemy";

    void Start()
    {
        coins = 10;
        health = 100;
        healthBar.SetMaxHealth(health);
        coinCounter.IncreaseCounter(coins);
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
        if (health <= 0) return; // FIXME: Placeholder to not keep executing this function even though the castle is destroyed until gameover is implemented
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    async void Die()
    {
        await AldenGenerator.LogAldenChat("Serpina died trying to save castle and now the monsters are coming for you");
    }
    void Attack()
    {
        animator.SetTrigger("Attack");

        // Transform newAttackPoint = transform.position + attackDirection * attackRange;

        // Detect enemies in range of the attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        Debug.Log(hitEnemies.ToString());
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag(enemyTag))
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<Enemy>().TakeDamage(10, this.transform); // Assuming 10 is the damage amount
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
