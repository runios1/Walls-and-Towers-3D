using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public HealthBar healthBar;
    public float damage = 10f;
    public float speed = 2f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    public bool navmash = false;
    void Start()
    {
        healthBar.SetMaxHealth(health);
        if(navmash)
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                Debug.LogError("NavMeshAgent component is missing!");
        if ((animator = GetComponent<Animator>())== null)
            Debug.LogError("Animator component is missing!");
        //agent.updatePosition = false;
        SetInitialTarget();
    }

    void Update()
    {
        if (target != null)
        {
            if(navmash)
                agent.destination=target.position;
            else
                MoveTowardsTarget();
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

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(direction);
        //animator.SetBool("isWalking", true);
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.SetHealth(health);
        //animator.SetTrigger("Get Hit");
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
            //animator.SetTrigger("Basic Attack");
            target.GetComponent<PlayerMainScript>().TakeDamage(damage);
        }
        else if (target.CompareTag("Tower"))
        {
            //animator.SetTrigger("HornAttack");
            target.GetComponent<Tower>().TakeDamage(damage);
        }
        else if (target.CompareTag("Core"))
        {
            //animator.SetTrigger("HornAttack");
            target.GetComponent<Castle>().TakeDamage(damage);
        }
    }

    private void Die()
    {
        //animator.SetTrigger("Die");
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
            Debug.Log("Initial target set to: Core");
        }
        else
        {
            Debug.LogError("Core object not found!");
        }
    }
    
}