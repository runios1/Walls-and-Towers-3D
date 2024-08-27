using UnityEngine;
using UnityEngine.AI;


public class BasicEnemy : BaseEnemy
{

    protected override void Start()
    {
        hyperParameters = new EnemyHyperParameters(100f, 10f, 2f, 1.5f, 1.2f);
        //in probability of 0.8, the target is the core, otherwise it is the player
        target = UnityEngine.Random.Range(0, 1.0f) <= 0.8f ? GameObject.FindGameObjectWithTag("Core").transform
         : GameObject.FindGameObjectWithTag("Player").transform;
         base.Start();
    }



    // public void ChangeState(IEnemyState newState)
    // {
    //     if (currentState != null)
    //         currentState.ExitState();
    //     currentState = newState;
    //     currentState.EnterState();
    // }




    // public void TakeDamage(float amount, Transform attacker)
    // {
    //     if (currentState.getState() == EnemyStateEnum.DYING)
    //         return;
    //     Debug.Log("Taking damage: " + amount);
    //     hyperParameters.health -= amount;
    //     healthBar.SetHealth(hyperParameters.health);

    //     SetAnimation(AnimationState.GET_HIT, 1);
    //     Debug.Log("Attacked by: " + attacker.name);
    //     currentState.OnHit(attacker);

    // }



    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Wall"))
    //     {
    //         Debug.Log("Reaching Wall: " + other.gameObject.name);
    //         currentState.OnReachingWall(other.gameObject.transform);
    //     }
    // }

    // //******* Handling Animations *******
    // void OnAnimatorMove()
    // {
    //     if (animator && agent && animator.rootPosition != null && agent.nextPosition != null)
    //     {
    //         Vector3 position = animator.rootPosition;
    //         position.y = agent.nextPosition.y;
    //         transform.position = position;
    //         //Debug.Log("Animator moved to position: " + position);
    //     }
    // }
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
            case AnimationState.ATTACK:
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
            case AnimationState.ATTACK:
                return "Basic Attack";
            case AnimationState.GET_HIT:
                return "GetHit";
            default:
                return "Unknown";
        }
    }

    internal override void TriggerMoveAnimation(float value)
    {
        animator.SetBool("Walk", value > 0);
    }

    internal override void TriggerAttackAnimation()
    {
        animator.SetTrigger("Basic Attack");
    }
    internal override void TriggerSpeedAnimation(float value){
        animator.SetFloat("speed", value);
    }
    internal override void TriggerGetHitAnimation()
    {
        animator.SetTrigger("GetHit");
    }

    internal override void TriggerDieAnimation()
    {
        animator.SetTrigger("Die");
    }

    internal override void ResetHitAttackAnimations(int reset)
    {
        // bitwise check. reset = xy where x and y are bits, if y is 1 the GetHit animation will be reset if x is 1 the Basic Attack animation will be reset
        //for example, if reset = 0, x=y=0 and then no animation will be reset. if reset =1 then x=0,y=1 then GetHit animation will be reset but Basic Attack animation will not be reset.
        //if reset =2 then x=1,y=0 and Basic Attack animation will be reset but GetHit animation will not be reset.
        //if reset =3 then x=y=1, then both animations will be reset.
        if((reset & 1) != 0){
            animator.ResetTrigger("GetHit");
        }
        if((reset & 2) != 0){
            animator.ResetTrigger("Basic Attack");
        }
    }
}
