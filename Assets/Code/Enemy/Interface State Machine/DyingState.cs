using UnityEngine;

public class DyingState : IEnemyState
{
    private Enemy enemy;
    private bool isDead = false;
    public DyingState(Enemy enemy){
        this.enemy = enemy;
    }
    public void EnterState()
    {
        //enemy.SetAnimation(AnimationState.DIE, 1);
        enemy.agent.isStopped = true;
        enemy.agent.ResetPath();
    }

    public void ExitState()
    {
        return;
    }

    public EnemyStateEnum getState()
    {
        return EnemyStateEnum.DYING;
    }

    public void OnHit(Transform attacker)
    {
        return;
    }
    public bool died(){
        return isDead;
    }
    public void UpdateState()
    {
        if(!isDead){
            isDead = true;
            enemy.Die();
            Debug.Log("Dying!");
        }
    }
}