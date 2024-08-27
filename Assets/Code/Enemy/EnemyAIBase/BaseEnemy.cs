using UnityEngine;
using UnityEngine.AI;
using System.Linq;


public class Enemy : MonoBehaviour
using System;
public abstract class BaseEnemy : MonoBehaviour
{
    protected IEnemyState currentState;

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
    protected ScoringSystem scoringSystem;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lookAtScript = FindObjectOfType<LookAt>();
        waveManager = FindObjectOfType<WaveManager>();
        player = FindObjectOfType<PlayerMainScript>();
        scoringSystem = FindObjectOfType<ScoringSystem>();

        healthBar.SetMaxHealth(hyperParameters.health);
        agent.stoppingDistance = hyperParameters.attackRange;
        head = transform.GetChild(4);

        ChangeState(new IdleState(this));
    }

    protected virtual void Update()
    {
        if (currentState != null)
        {
            if (currentState.getState() == EnemyStateEnum.DYING && ((DyingState)currentState).died())
                return;
            else
                currentState.UpdateState();
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
            currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public virtual void TakeDamage(float amount, Transform attacker)
    {
        if (currentState.getState() == EnemyStateEnum.DYING)
            return;

        hyperParameters.health -= amount;
        healthBar.SetHealth(hyperParameters.health);

        TriggerGetHitAnimation();
        currentState.OnHit(attacker);
    }

    public virtual void SelectNextTarget()
    {
        float playerDistance = player != null && player.health > 0 ? Vector3.Distance(transform.position, player.transform.position) : float.MaxValue;
        Tower[] towers = FindObjectsOfType<Tower>()
            .Where(tower => tower.state == Placeable.PlaceableState.Placed)
            .ToArray();

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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && other.gameObject.GetComponent<BasicWall>().state == Placeable.PlaceableState.Placed)
        {
            currentState.OnReachingWall(other.gameObject.transform);
        }
    }

    protected virtual void OnAnimatorMove()
    {
        if (animator && agent && animator.rootPosition != null && agent.nextPosition != null)
        {
            Vector3 position = animator.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
        }
    }

    // Abstract methods for animations, to be implemented by each specific enemy type
    internal abstract void TriggerMoveAnimation(float value);
    internal abstract void TriggerSpeedAnimation(float value);
    internal abstract void TriggerAttackAnimation();
    internal abstract void TriggerGetHitAnimation();
    internal abstract void TriggerDieAnimation();

    public virtual void Die()
    {
        ResetHitAttackAnimations(3);
        TriggerDieAnimation();

        if (waveManager)
            waveManager.UnregisterEnemy();
        player.GetCoins(1);
        scoringSystem.EnemyKilled();
        Destroy(gameObject, 1.3f);
        this.enabled = false;
    }

    // Reset triggers for hit and attack animations
    internal abstract void ResetHitAttackAnimations(int reset);
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
    ATTACK,
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