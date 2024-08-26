using System;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    IEnemyState currentState;

    public NavMeshAgent agent;
    public Transform target;
    internal Transform previousTarget;
    public Animator animator;
    public LookAt lookAtScript;
    public WaveManager waveManager;
    public HealthBar healthBar;
    public PlayerMainScript player;
    public EnemyHyperParameters hyperParameters;
    public Transform head;

    [HideInInspector]
    public AudioSource audioSource;
    [Header("Audio")]
    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip dieSound;
    private ScoringSystem scoringSystem;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        //agent.acceleration = 1f;

        animator = GetComponent<Animator>();
        LookAt lookAtScript = FindAnyObjectByType<LookAt>();
        waveManager = FindAnyObjectByType<WaveManager>();
        player = FindAnyObjectByType<PlayerMainScript>();
        hyperParameters = new EnemyHyperParameters(100f, 10f, 2f, 1.5f, 1.2f);
        healthBar.SetMaxHealth(hyperParameters.health);
        //target = GameObject.FindGameObjectWithTag("Core").transform;
        //in probability of 0.8, the target is the core, otherwise it is the player
        target = UnityEngine.Random.Range(0, 1.0f) <= 0.8f ? GameObject.FindGameObjectWithTag("Core").transform
         : GameObject.FindGameObjectWithTag("Player").transform;
        agent.stoppingDistance = hyperParameters.attackRange;
        //agent.autoBraking=true;
        head = transform.GetChild(4);

        scoringSystem = FindObjectOfType<ScoringSystem>();
        ChangeState(new IdleState(this));
    }


    void Update()
    {
        if (currentState != null)
            if (currentState.getState() == EnemyStateEnum.DYING && ((DyingState)currentState).died())
                return;
            else
                currentState.UpdateState();
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }




    public void TakeDamage(float amount, Transform attacker)
    {
        if (currentState.getState() == EnemyStateEnum.DYING)
            return;
        Debug.Log("Taking damage: " + amount);
        hyperParameters.health -= amount;
        healthBar.SetHealth(hyperParameters.health);

        SetAnimation(AnimationState.GET_HIT, 1);
        Debug.Log("Attacked by: " + attacker.name);
        currentState.OnHit(attacker);

    }

    public void SelectNextTarget()
    {
        float playerDistance = player != null && player.health > 0 ? Vector3.Distance(transform.position, player.transform.position) : float.MaxValue;
        Tower[] towers = FindObjectsOfType<Tower>();

        Array.Sort(towers, (a, b) =>
        {
            float dist = Vector3.Distance(transform.position, a.transform.position) - Vector3.Distance(transform.position, b.transform.position);
            return dist.CompareTo(0);
        });
        float towerDistance = towers.Length != 0 ? Vector3.Distance(towers[0].transform.position, transform.position)
        : float.MaxValue;
        GameObject coreObject = GameObject.FindGameObjectWithTag("Core");
        float castleDistance = coreObject != null ? Vector3.Distance(coreObject.transform.position, transform.position) : float.MaxValue;
        float closestDistance = Mathf.Min(playerDistance, towerDistance, castleDistance);
        if (closestDistance == playerDistance)
            target = player.transform;

        else if (closestDistance == towerDistance)
        {
            target = towers[0].transform;
        }
        else
        {
            target = coreObject.transform;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Reaching Wall: " + other.gameObject.name);
            currentState.OnReachingWall(other.gameObject.transform);
        }
    }

    //******* Handling Animations *******
    void OnAnimatorMove()
    {
        if (animator && agent && animator.rootPosition != null && agent.nextPosition != null)
        {
            Vector3 position = animator.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
            //Debug.Log("Animator moved to position: " + position);
        }
    }
    public void SetAnimation(AnimationState state, float value)
    {
        switch (state)
        {
            case AnimationState.SPEED:
                animator.SetFloat("speed", value);
                break;
            case AnimationState.WALK:
                animator.SetBool("Walk", value > 0);
                break;
            case AnimationState.BASIC_ATTACK:
                animator.SetTrigger("Basic Attack");
                //animator.SetBool("Attack", value > 0);
                break;
            case AnimationState.GET_HIT:
                animator.SetTrigger("GetHit");
                break;
            case AnimationState.DIE:
                animator.SetTrigger("Die");
                break;
            default:
                Debug.LogError("Unknown animation state: " + state);
                break;
        }
    }
    private string StringFromAnimatorState(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.WALK:
                return "Walk";
            case AnimationState.SPEED:
                return "speed";
            case AnimationState.BASIC_ATTACK:
                return "Basic Attack";
            case AnimationState.GET_HIT:
                return "GetHit";
            default:
                return "Unknown";
        }
    }

    public void Die()
    {
        animator.ResetTrigger("GetHit");
        animator.ResetTrigger("Basic Attack");
        SetAnimation(AnimationState.DIE, 1);

        if (waveManager)
            waveManager.UnregisterEnemy();
        player.GetCoins(1);
        scoringSystem.EnemyKilled();
        Destroy(gameObject, 1.3f);
        this.enabled = false;
    }
}
public enum EnemyStateEnum
{
    IDLE,
    MOVING,
    ATTACKING,
    DYING
}

public struct EnemyHyperParameters
{
    public float health;

    public readonly float damage;
    public readonly float speed;
    public readonly float attackRange;
    public readonly float attackCooldown;

    public EnemyHyperParameters(float health, float damage, float speed, float attackRange, float attackCooldown)
    {
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.attackRange = attackRange;
        this.attackCooldown = attackCooldown;
    }
}
public enum AnimationState
{
    WALK,
    SPEED,
    BASIC_ATTACK,
    GET_HIT,
    DIE
}
public interface IEnemyState
{

    void EnterState();
    void UpdateState();
    void ExitState();
    void OnHit(Transform attacker);
    void OnReachingWall(Transform wall);

    EnemyStateEnum getState();
}