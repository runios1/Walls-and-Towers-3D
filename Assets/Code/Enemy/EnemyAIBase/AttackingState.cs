using UnityEngine;

public class AttackingState : IEnemyState
{
    private BaseEnemy enemy;
    //private Vector3 attackPosition;
    private float lastAttackTime;
    public AttackingState(BaseEnemy enemy)
    {
        this.enemy = enemy;
        lastAttackTime = Time.time;

    }
    public void EnterState()
    {
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK,1);
        enemy.audioSource.clip = enemy.attackSound;
        enemy.audioSource.loop = true;
        enemy.audioSource.Play();
    }

    public void ExitState()
    {
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK,0);
        Debug.Log("Attacking State Exit");
        if (enemy.audioSource.clip = enemy.attackSound) enemy.audioSource.Stop();
    }

    public EnemyStateEnum getState()
    {
        return EnemyStateEnum.ATTACKING;
    }

    public void OnHit(Transform attacker)
    {
        if (enemy.hyperParameters.health <= 0)
        {
            enemy.ChangeState(new DyingState(enemy));
            return;
        }
        if (enemy.target == null || enemy.target.CompareTag("Wall") || (!enemy.target.CompareTag("Core") && !enemy.target.gameObject == attacker.gameObject))
        {
            enemy.target = attacker;
            if (enemy.target.CompareTag("Wall"))
            {
                enemy.previousTarget = null;
            }
            Debug.Log("New target selected: " + attacker.name);
            enemy.ChangeState(new MovingState(enemy));
        }
    }
    public void OnReachingWall(Transform wall)
    {
        return;
    }

    public void UpdateState()
    {
        Transform target = enemy.target;
        float damage = enemy.hyperParameters.damage;
        if (target == null)
        {
            enemy.ChangeState(new IdleState(enemy));
            return;
        }
        if (Time.time < lastAttackTime + enemy.hyperParameters.attackCooldown)
            return;
        if (enemy.lookAtScript != null)
        {
            enemy.lookAtScript.lookAtTargetPosition = target.position;
        }

        BoxCollider collider = target.GetComponent<BoxCollider>();
        if (collider == null)
            return;
        Vector3 closestPoint = collider.ClosestPoint(enemy.head.transform.position);
        if (target.CompareTag("Player") && Vector3.Distance(enemy.head.transform.position, closestPoint) > enemy.hyperParameters.attackRange)
        {
            //Debug.Log("Target is out of range");
            enemy.ChangeState(new MovingState(enemy));
            return;
        }
        lastAttackTime = Time.time;
        Debug.Log("ATTACKING! Target: " + (target != null ? target.name : "None"));
        float health = float.MaxValue;
        //enemy.animator.ResetTrigger("GetHit");
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK, 1);
        enemy.ResetHitAttackAnimations(1);
        enemy.TriggerAttackAnimation();
        if (target.CompareTag("Player"))
        {
            target.GetComponent<PlayerMainScript>().TakeDamage(damage);
            health = target.GetComponent<PlayerMainScript>().health;
        }
        else if (target.CompareTag("Tower"))
        {
            target.GetComponent<Tower>().TakeDamage(damage);
            health = target.GetComponent<Tower>().health;
        }
        else if (target.CompareTag("Core"))
        {
            target.GetComponent<Castle>().TakeDamage(damage);
            health = target.GetComponent<Castle>().health;
        }
        else if (target.CompareTag("Wall"))
        {
            target.GetComponent<BasicWall>().TakeDamage(damage);
            health = target.GetComponent<BasicWall>().health;
        }
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK,0);
        if (health <= 0)
        {
            if (target.CompareTag("Wall"))
            {
                if (enemy.previousTarget != null)
                {
                    enemy.target = enemy.previousTarget;
                    enemy.previousTarget = null;
                    enemy.ChangeState(new MovingState(enemy));
                }
            }
            enemy.ChangeState(new IdleState(enemy));
        }
    }
}
