using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;
    public IdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void EnterState()
    {
        enemy.SetAnimation(AnimationState.WALK, 0);
        enemy.SetAnimation(AnimationState.BASIC_ATTACK, 0);
        enemy.animator.ResetTrigger("GetHit");
        //enemy.animator.ResetTrigger("Basic Attack");
    }

    public void ExitState()
    {
        return;
    }
    public void OnReachingWall(Transform wall)
    {
        enemy.target = wall;
        enemy.ChangeState(new MovingState(enemy));
    }

    public EnemyStateEnum getState()
    {
        return EnemyStateEnum.IDLE;
    }

    public void OnHit(Transform attacker)
    {
        if (enemy.hyperParameters.health <= 0)
        {
            enemy.ChangeState(new DyingState(enemy));
            return;
        }
        enemy.target = attacker;
        Debug.Log("New target selected: " + attacker.name);
        enemy.ChangeState(new MovingState(enemy));
    }

    public void UpdateState()
    {
        Transform target = enemy.target;
        GameObject targetObject = target == null ? null :
                                    target.CompareTag("Player") ? target.GetComponent<PlayerMainScript>().gameObject :
                                    target.CompareTag("Tower") ? target.GetComponent<Tower>().gameObject :
                                    target.CompareTag("Core") ? target.GetComponent<Castle>().gameObject : null;
        if (targetObject != null)
            enemy.ChangeState(new MovingState(enemy));
        else
            enemy.SelectNextTarget();
    }
}