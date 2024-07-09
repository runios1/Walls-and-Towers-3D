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
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private bool debug = true;
    void Start()
    {
        healthBar.SetMaxHealth(health);
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("NavMeshAgent component is missing!");
        if ((animator = GetComponent<Animator>())== null)
            Debug.LogError("Animator component is missing!");
        //agent.updatePosition = false;
        if(!debug)
            SetInitialTarget();
    }

    void Update()
    {
        if (target != null)
        {
            // if(!agent.SetDestination(target.position))
            //     Debug.LogError("couldn't set destintaion");
            // animator.SetTrigger("Walk");
            agent.destination=target.position;
            //Debug.Log("Moving towards target: " + target.name+"at position:" +target.position);
            if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }else{
            //animator.SetTrigger("Idle");
            Debug.Log("No target available.");
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        animator.SetTrigger("Get Hit");
        Debug.Log("Took damage: " + amount + ", current health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        Debug.Log("ATTACKING!");
        if (target.CompareTag("Player"))
        {
            animator.SetTrigger("Basic Attack");
            target.GetComponent<PlayerMainScript>().TakeDamage(damage);
        }
        else if (target.CompareTag("Tower"))
        {
            animator.SetTrigger("HornAttack");
            target.GetComponent<Tower>().TakeDamage(damage);
        }
        else if (target.CompareTag("Core"))
        {
            animator.SetTrigger("HornAttack");
            target.GetComponent<Castle>().TakeDamage(damage);
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log("Dying...");
        Destroy(gameObject,2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tower"))
        {
            target = other.transform;
            Debug.Log("New target acquired: " + target.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target == other.transform)
        {
            if(!debug)
                SetInitialTarget();
            Debug.Log("Reverting to initial target: Core");
        }
    }

    private void SetInitialTarget()
    {
        GameObject coreObject = GameObject.FindGameObjectWithTag("Core");

        if (coreObject != null)
        {
            target = coreObject.transform;
            //agent.destination = target.position;
            Debug.Log("Initial target set to: Core");
            //Debug.Log("target coordinates:"+target.position.ToString() + "Enemy coordinates:"+GetComponent<Transform>().position.ToString());
        }
        else
        {
            Debug.LogError("Core object not found!");
        }
    }
    
}
