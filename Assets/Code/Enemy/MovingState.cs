using UnityEngine;

public class MovingState : IEnemyState
{
    private Enemy enemy;
    private Transform target;
    private Vector3 closestPoint;
    private Vector3 currentTargetPosition;
    public MovingState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void EnterState()
    {
        this.enemy.agent.isStopped = false;
        enemy.agent.ResetPath();
        target = enemy.target;
        BoxCollider collider = target.GetComponent<BoxCollider>();
        closestPoint = collider.ClosestPoint(enemy.head.transform.position);
        currentTargetPosition = enemy.transform.position;
        Debug.DrawLine(enemy.transform.position, closestPoint, Color.red);
        //add a bit of noise to the target position to avoid getting stuck in corners
        //closestPoint += new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        //Debug.Log("closestPoint: " + closestPoint);
        enemy.SetAnimation(AnimationState.WALK, 1);
        enemy.SetAnimation(AnimationState.SPEED, enemy.hyperParameters.speed);

        enemy.audioSource.clip = enemy.walkSound;
        enemy.audioSource.loop = true;
        enemy.audioSource.Play();
    }

    public void ExitState()
    {
        this.enemy.agent.isStopped = true;
        enemy.agent.ResetPath();

        this.enemy.SetAnimation(AnimationState.WALK, 0);
        enemy.SetAnimation(AnimationState.SPEED, 0);

        if (enemy.audioSource.clip = enemy.walkSound) enemy.audioSource.Stop();
    }

    public EnemyStateEnum getState()
    {
        return EnemyStateEnum.MOVING;
    }

    public void OnHit(Transform attacker)
    {
        if (enemy.hyperParameters.health <= 0)
        {
            enemy.ChangeState(new DyingState(enemy));
            return;
        }
        if (enemy.target != attacker)
        {
            enemy.target = attacker;
            Debug.Log("New target selected: " + attacker.name);
            enemy.ChangeState(new MovingState(enemy));
        }
    }
    public void OnReachingWall(Transform wall)
    {
        enemy.previousTarget = enemy.target;
        enemy.target = wall;
        enemy.ChangeState(new AttackingState(enemy));
    }

    public void UpdateState()
    {
        if (IsAtTarget())
        {
            this.enemy.ChangeState(new AttackingState(enemy));
        }
        else
        {
            if (enemy.target.transform.position != currentTargetPosition)
            {
                BoxCollider collider = target.GetComponent<BoxCollider>();
                closestPoint = collider.ClosestPoint(enemy.head.transform.position);
                currentTargetPosition = enemy.transform.position;
                enemy.agent.acceleration = 5;
            }
            this.enemy.agent.SetDestination(closestPoint);
            float agentSpeed = enemy.agent.velocity.magnitude;
            float animationSpeedMultiplier = agentSpeed / enemy.hyperParameters.speed;
            enemy.animator.SetFloat("speed", animationSpeedMultiplier);
        }
    }
    private bool IsAtTarget()
    {
        return Vector3.Distance(enemy.head.transform.position, closestPoint) < enemy.hyperParameters.attackRange;
    }
}