using UnityEngine;

public class MovingState : IEnemyState
{
    private Enemy enemy;
    private Transform target;
    private Vector3 closestPoint;
    public MovingState(Enemy enemy){
        this.enemy = enemy;
    }
    public void EnterState()
    {
        this.enemy.agent.isStopped = false;
        enemy.agent.ResetPath();
        target = enemy.target;
        Collider collider = target.GetComponent<Collider>();
        closestPoint = collider.ClosestPoint(enemy.head.transform.position);
        Debug.Log("closestPoint: " + closestPoint);
        enemy.SetAnimation(AnimationState.WALK, 1);
        enemy.SetAnimation(AnimationState.SPEED,enemy.hyperParameters.speed);
        
    }

    public void ExitState()
    {
        this.enemy.agent.isStopped = true;
        enemy.agent.ResetPath();
        
        this.enemy.SetAnimation(AnimationState.WALK, 0);
        enemy.SetAnimation(AnimationState.SPEED,0);
    }

    public EnemyStateEnum getState()
    {
        return EnemyStateEnum.MOVING;
    }

    public void OnHit(Transform attacker)
    {
        if(enemy.hyperParameters.health <= 0){
            enemy.ChangeState(new DyingState(enemy));
            return;
        }
        //if(enemy.target == null){
            enemy.target = attacker;
            Debug.Log("New target selected: " + attacker.name);
            enemy.ChangeState(new MovingState(enemy));
        //}
    }

    public void UpdateState()
    {
        if(IsAtTarget()){
            this.enemy.ChangeState(new AttackingState(enemy));
        }else{
            this.enemy.agent.SetDestination(closestPoint);
            float agentSpeed = enemy.agent.velocity.magnitude;
            float animationSpeedMultiplier = agentSpeed / enemy.hyperParameters.speed;
            //enemy.SetAnimation(AnimationState.WALK, 1);
            enemy.animator.SetFloat("speed", animationSpeedMultiplier);
        }
    }
    private bool IsAtTarget(){
        return Vector3.Distance(enemy.head.transform.position, closestPoint) < enemy.hyperParameters.attackRange;
    }
}