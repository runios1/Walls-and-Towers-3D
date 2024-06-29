using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;
    private Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Set the initial target to the core
        target = GameObject.FindGameObjectWithTag("Core").transform;
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
        if (health <= 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        // Implement attack logic (e.g., dealing damage to the target)
        if (target.CompareTag("Player"))
        {
            target.GetComponent<Player>().TakeDamage(damage);
        }
        else if (target.CompareTag("Tower"))
        {
            target.GetComponent<Tower>().TakeDamage(damage);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tower"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target == other.transform)
        {
            // Revert to targeting the core
            target = GameObject.FindGameObjectWithTag("Core").transform;
        }
    }
}

internal class Tower
{
    internal void TakeDamage(float damage)
    {
        throw new NotImplementedException();
    }
}

internal class Player
{
    internal void TakeDamage(float damage)
    {
        throw new NotImplementedException();
    }
}