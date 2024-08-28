using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDragon : BaseEnemy
{
    public GameObject AttackParticales;
    public GameObject Canvas;
    public GameObject HUD;
    internal override void ResetHitAttackAnimations(int reset)
    {
        if ((reset & 1) != 0)
        {
            animator.ResetTrigger("Get Hit");
        }
        if ((reset & 2) != 0)
        {
          
            animator.ResetTrigger("Attack");
        }
    }

    internal override void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
        
    }
  
   
    internal override void TriggerDieAnimation()
    {
        animator.SetTrigger("Die");
    }

    internal override void TriggerGetHitAnimation()
    {
        animator.SetTrigger("Get Hit");
    }

    internal override void TriggerMoveAnimation(float value)
    {
        // if (value < 0){
        //     animator.SetFloat("Speed", 0);
        // }
    }

    internal override void TriggerSpeedAnimation(float value)
    {
        // if(value == 0)
        //     animator.SetFloat("Speed",0);
        // else
        if (value == -1)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
            Debug.Log("agent.velocity.magnitude:" + agent.velocity.magnitude);
        }
    }

    // Start is called before the first frame update
    override protected void Start()
    {

        hyperParameters = new EnemyHyperParameters(100f, 5f, 20f, GetComponent<NavMeshAgent>().stoppingDistance, 3.3f);
     
         //in probability of 0.8, the target is the core, otherwise it is the player
        target = UnityEngine.Random.Range(0, 1.0f) <= 0.8f ? GameObject.FindGameObjectWithTag("Core").transform
         : GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    //Update is called once per frame
    override protected void Update()
    {
        //Debug.Log("Fast Enemy State: "+ currentState.getState());
        base.Update();
        TriggerSpeedAnimation(-1);
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        lookAtScript.lookAtTargetPosition = agent.steeringTarget + transform.forward;
        if (worldDeltaPosition.magnitude > agent.radius)
            agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
    }
}
