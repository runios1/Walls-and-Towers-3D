using System;
using System.Collections;
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
    private WaveManager waveManager;
    private PlayerMainScript player;
    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
        player = FindObjectOfType<PlayerMainScript>();
        healthBar.SetMaxHealth(health);
        if (navmash)
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
                Debug.LogError("NavMeshAgent component is missing!");
        }
        if ((animator = GetComponent<Animator>()) == null)
            Debug.LogError("Animator component is missing!");
        SetInitialTarget();
    }

    void Update()
    {
        // if (target != null)
        // {
        //     if (navmash){
        //         agent.destination = target.position;
        //         if(!animator.GetBool("Walk 0"))
        //             animator.SetBool("Walk 0",true);
        //     }
        //     else
        //         MoveTowardsTarget();
        //     //Debug.Log("Moving towards target: " + target.name+"at position:" +target.position);
        //     if (Vector3.Distance(transform.position, target.position) <= attackRange && Time.time > lastAttackTime + attackCooldown)
        //     {
        //         if(animator.GetBool("Walk 0"))
        //             animator.SetBool("Walk 0",false);
        //         Attack();
        //         lastAttackTime = Time.time;
        //     }
        // }
        // else
        // {
        //     animator.SetBool("Walk 0",false);
        //     Debug.Log("No target available, Going for the core");
        //     GameObject coreObject = GameObject.FindGameObjectWithTag("Core");
        //     target = coreObject.transform;
        //     agent.destination = target.position;
        // }
        if(target != null){
            if(Vector3.Distance(agent.transform.position, target.position) > agent.stoppingDistance)
                animator.SetBool("Walk 0",true);
            else{
                animator.SetBool("Walk 0",false);
                if(Time.time > lastAttackTime + attackCooldown){
                    Attack();
                    lastAttackTime=Time.time;
                }
                
            }
            agent.SetDestination(target.position);
        }
        else{
            animator.SetBool("Walk 0",false);
            Debug.Log("No target available, choosing another target");
            SelectNextTarget();
        }
    }


    private void SelectNextTarget()
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        // Check player
        if (player != null && player.health > 0)
        {
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (playerDistance < closestDistance && playerDistance <= attackRange)
            {
                closestTarget = player.transform;
                closestDistance = playerDistance;
            }
        }

        // Check towers
        Tower[] towers = FindObjectsOfType<Tower>();
        foreach (Tower tower in towers)
        {
            if (tower.health > 0)
            {
                float towerDistance = Vector3.Distance(transform.position, tower.transform.position);
                if (towerDistance < closestDistance && towerDistance <= attackRange)
                {
                    closestTarget = tower.transform;
                    closestDistance = towerDistance;
                }
            }
        }

        // Check core
        GameObject coreObject = GameObject.FindGameObjectWithTag("Core");
        if (coreObject != null)
        {
            float coreDistance = Vector3.Distance(transform.position, coreObject.transform.position);
            if (coreDistance < closestDistance)
            {
                closestTarget = coreObject.transform;
            }
        }

        target = closestTarget;
    }
    // private void MoveTowardsTarget()
    // {
    //     Vector3 direction = (target.position - transform.position).normalized;
    //     transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    //     transform.rotation = Quaternion.LookRotation(direction);
    //     //animator.SetBool("isWalking", true);
    // }
    public void TakeDamage(float amount, Transform attacker)
    {
        health -= amount;
        healthBar.SetHealth(health);
        Debug.Log("Took damage: " + amount + ", current health: " + health);
        
        // Trigger the "Get Hit" animation
        animator.SetTrigger("GetHit");

        if (health <= 0)
        {
            Die();
        }
        else if(target == null){
            target = attacker;
            Debug.Log("Updated target to attacker: " + attacker.name);
        }else{
            StartCoroutine(WaitForAnimation("GetHit",2f));
        }
    }

    private IEnumerator WaitForAnimation(string animationName, float delay)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(delay);
    }

    private void Attack()
    {
        animator.SetBool("Attack",true);
        Debug.Log("ATTACKING!");
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
        StartCoroutine(ResetAttackBool());
    }
    private IEnumerator ResetAttackBool()
    {
        // Wait for a short duration to ensure the attack animation has time to play
        yield return new WaitForSeconds(1); // Adjust this time based on the length of your attack animation
        animator.SetBool("Attack", false);
    }
    private void Die()
    {
        Debug.Log("Dying...");

        // Triggering the animation
        animator.SetTrigger("Die");

        //disable further movements
        agent.isStopped = true;
        this.enabled = false;

        //Unregister the enemy
        waveManager.UnregisterEnemy();
        player.GetCoins(1);
        Destroy(gameObject,2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Tower"))
        {
            target = other.transform;
            Debug.Log("New target acquired: " + target.name);
            Attack();
        }
        else if (other.CompareTag("Core"))
        {
            Attack();
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
    void OnAnimatorMove ()
    {
        if(agent != null && agent.nextPosition != null)
            // Update position to agent position
            transform.position = agent.nextPosition;
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