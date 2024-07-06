using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public HealthBar healthBar;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;
    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        healthBar.SetMaxHealth(health);

        agent = GetComponent<NavMeshAgent>();
        SetInitialTarget();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerMainScript>().TakeDamage(damage);
        }
        else if (target.CompareTag("Tower"))
        {
            target.GetComponent<Tower>().TakeDamage(damage);
        }
        else if (target.CompareTag("Core"))
        {
            target.GetComponent<Castle>().TakeDamage(damage);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tower") || other.CompareTag("Wall"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target == other.transform)
        {
            SetInitialTarget();
        }
    }

    private void SetInitialTarget()
    {
        target = GameObject.FindGameObjectWithTag("Core").transform;
    }
}