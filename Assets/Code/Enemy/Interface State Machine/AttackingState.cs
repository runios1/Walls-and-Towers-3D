using Unity.VisualScripting;
using UnityEngine;

public class AttackingState : IEnemyState
{
    private Enemy enemy;
    private float lastAttackTime;
    public AttackingState(Enemy enemy){
        this.enemy = enemy;
        lastAttackTime = Time.time;
    }
    public void EnterState()
    {
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK,1);
    }

    public void ExitState()
    {
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK,0);
        Debug.Log("Attacking State Exit");
    }

    public EnemyStateEnum getState()
    {
        return EnemyStateEnum.ATTACKING;
    }

    public void OnHit(Transform attacker)
    {
        if(enemy.hyperParameters.health <= 0){
        enemy.ChangeState(new DyingState(enemy));
        return;
        }
        if((enemy.target == null || !enemy.target.CompareTag("Core")) && !enemy.target.gameObject == attacker.gameObject){
            enemy.target = attacker;
            Debug.Log("New target selected: " + attacker.name);
            enemy.ChangeState(new MovingState(enemy));
        }
    }

    public void UpdateState()
    {
        Transform target = enemy.target;
        float damage = enemy.hyperParameters.damage;
        if(target == null)
            return;
        if(Time.time < lastAttackTime + enemy.hyperParameters.attackCooldown)
            return;

        lastAttackTime = Time.time;
        Debug.Log("ATTACKING! Target: " + (target != null ? target.name : "None"));
        float health = float.MaxValue;
        enemy.SetAnimation(AnimationState.BASIC_ATTACK,1);
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
        //enemy.SetAnimation(AnimationState.BASIC_ATTACK,0);
        if(health <= 0){
            enemy.ChangeState(new IdleState(enemy));
        }
    }
}
