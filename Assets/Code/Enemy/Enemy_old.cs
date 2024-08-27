
// // void Update()
// // {
// //     if (target != null)
// //     {
// //         if (Vector3.Distance(agent.transform.position, target.position) > agent.stoppingDistance)
// //         {
// //             //Debug.Log("agent velocity and speed: "+agent.velocity+ " , "+agent.velocity.magnitude);
// //             float agentSpeed = agent.velocity.magnitude;
// //             float animationSpeedMultiplier = agentSpeed / speed;
// //             animator.SetFloat("speed", animationSpeedMultiplier);
// //             animator.SetBool("Walk 0", true);
// //         }
// //         else
// //         {
// //             agent.isStopped = true;
// //             animator.SetBool("Walk 0", false);
// //             if (Time.time > lastAttackTime + attackCooldown)
// //             {
// //                 Attack();
// //                 lastAttackTime = Time.time;
// //             }

// //         }

// //         agent.SetDestination(target.position);
// //     }
// //     else
// //     {
// //         animator.SetBool("Walk 0", false);
// //         agent.isStopped=false;
// //         Debug.Log("No target available, choosing another target");
// //         SelectNextTarget();
// //     }
// // }
// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.AI;
// public class Enemy : MonoBehaviour
// {
//     public float health = 100f;
//     public HealthBar healthBar;
//     public float damage = 10f;
//     public float speed = 2f;
//     public float attackRange = 2f;
//     public float attackCooldown = 1.5f;
//     private float lastAttackTime;
//     public Transform target;
//     private NavMeshAgent agent;
//     private Animator animator;
//     public bool navmash = false;
//     private WaveManager waveManager;
//     private PlayerMainScript player;
//     private bool isTargetingCore = false;
//     private LookAt lookAtScript;
//     private bool isAttacking = false;

//     void Start()
//     {
//         Debug.Log("Enemy Start called.");
//         lookAtScript = FindObjectOfType<LookAt>();
//         waveManager = FindObjectOfType<WaveManager>();
//         player = FindObjectOfType<PlayerMainScript>();
//         healthBar.SetMaxHealth(health);
//         if (navmash)
//         {
//             agent = GetComponent<NavMeshAgent>();
//             agent.stoppingDistance = attackRange;
//             agent.acceleration = 1;
//             if (agent == null)
//                 Debug.LogError("NavMeshAgent component is missing!");
//             else
//                 Debug.Log("NavMeshAgent initialized with stoppingDistance: " + attackRange);
//         }
//         if ((animator = GetComponent<Animator>()) == null)
//             Debug.LogError("Animator component is missing!");
//         else
//             Debug.Log("Animator component found.");

//         SetInitialTarget();
//     }

//     void Update()
//     {
//         Debug.Log("Enemy Update called. Target: " + (target != null ? target.name : "None"));
//         if (target != null && !isAttacking)
//         {
//             Debug.Log("Not attacking. Checking distance to target...");
//             if (agent.destination == target.position && !agent.pathPending && agent.remainingDistance > 0 && Vector3.Distance(agent.transform.position, target.position) <= agent.stoppingDistance)
//             {
//                 Debug.Log("Agent close to target. Stopping and preparing to attack.");
//                 agent.isStopped = true;
//                 agent.velocity = Vector3.zero;
//                 animator.SetFloat("speed", 0);

//                 if (Time.time > lastAttackTime + attackCooldown)
//                 {
//                     Attack();
//                     lastAttackTime = Time.time;
//                 }
//             }
//             else
//             {
//                 Debug.Log("Moving towards target. Setting destination.");
//                 if (agent.destination != target.position)
//                 {
//                     agent.SetDestination(target.position);
//                 }
//                 agent.isStopped = false;

//                 float agentSpeed = agent.velocity.magnitude;
//                 float animationSpeedMultiplier = agentSpeed / speed;
//                 animator.SetFloat("speed", animationSpeedMultiplier);

//                 animator.SetBool("Walk 0", true);
//             }
//         }
//         else if (target != null && isAttacking)
//         {
//             Debug.Log("Currently attacking. Keeping agent stopped.");
//             agent.isStopped = true;
//             agent.velocity = Vector3.zero;
//             animator.SetFloat("speed", 0);

//             if (Time.time > lastAttackTime + attackCooldown)
//             {
//                 Attack();
//                 lastAttackTime = Time.time;
//             }
//         }
//         else
//         {
//             Debug.Log("No target or attack ongoing. Selecting new target.");
//             animator.SetBool("Walk 0", false);
//             SelectNextTarget();
//         }
//     }

//     private void SelectNextTarget()
//     {
//         Debug.Log("Selecting next target...");
//         Transform closestTarget = null;
//         float closestDistance = float.MaxValue;
//         isTargetingCore = false;

//         if (player != null && player.health > 0)
//         {
//             float playerDistance = Vector3.Distance(transform.position, player.transform.position);
//             Debug.Log("Player distance: " + playerDistance);
//             if (playerDistance < closestDistance && playerDistance <= attackRange)
//             {
//                 closestTarget = player.transform;
//                 closestDistance = playerDistance;
//             }
//         }

//         Tower[] towers = FindObjectsOfType<Tower>();
//         foreach (Tower tower in towers)
//         {
//             if (tower.health > 0)
//             {
//                 float towerDistance = Vector3.Distance(transform.position, tower.transform.position);
//                 Debug.Log("Tower distance: " + towerDistance);
//                 if (towerDistance < closestDistance && towerDistance <= attackRange)
//                 {
//                     closestTarget = tower.transform;
//                     closestDistance = towerDistance;
//                 }
//             }
//         }

//         GameObject coreObject = GameObject.FindGameObjectWithTag("Core");
//         if (coreObject != null)
//         {
//             float coreDistance = Vector3.Distance(transform.position, coreObject.transform.position);
//             Debug.Log("Core distance: " + coreDistance);
//             if (coreDistance < closestDistance)
//             {
//                 closestTarget = GetCoreTarget(coreObject);
//                 closestDistance = coreDistance;
//                 isTargetingCore = true;
//             }
//         }

//         target = closestTarget;
//         Debug.Log("Next target selected: " + (target != null ? target.name : "None"));
//     }

//     // public void TakeDamage(float amount, Transform attacker)
//     // {
//     //     if(this.IsDestroyed())
//     //         return;
//     //     Debug.Log("Taking damage: " + amount);
//     //     health -= amount;
//     //     healthBar.SetHealth(health);

//     //     animator.SetTrigger("GetHit");

//     //     if (health <= 0)
//     //     {
//     //         Die();
//     //     }
//     //     else if (target == null)
//     //     {
//     //         target = attacker;
//     //         isTargetingCore = false;
//     //         if (lookAtScript != null)
//     //         {
//     //             lookAtScript.lookAtTargetPosition = attacker.position;
//     //         }
//     //         Debug.Log("Updated target to attacker: " + attacker.name);
//     //         if(!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
//     //             StartCoroutine(WaitForAnimation("GetHit", 1.1f));
//     //     }
//     //     else
//     //     {
//     //         if(!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
//     //             StartCoroutine(WaitForAnimation("GetHit", 1.1f));
//     //     }
//     // }

//     public void TakeDamage(float amount, Transform attacker)
//     {
//         if (this.IsDestroyed())
//             return;

//         Debug.Log("Taking damage: " + amount);
//         health -= amount;
//         healthBar.SetHealth(health);

//         animator.SetTrigger("GetHit");

//         if (health <= 0)
//         {
//             Die();
//         }
//         else
//         {
//             // Update the target to the attacker only if not currently attacking the core
//             if (!isTargetingCore)
//             {
//                 target = attacker;
//                 isTargetingCore = false;
//                 agent.isStopped = false;
//                 isAttacking = false;
//                 if (lookAtScript != null)
//                 {
//                     lookAtScript.lookAtTargetPosition = attacker.position;
//                 }
//                 Debug.Log("Updated target to attacker: " + attacker.name);
//             }

//             if (!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
//                 StartCoroutine(WaitForAnimation("GetHit", 1.1f));
//         }
//     }

//     private IEnumerator WaitForAnimation(string animationName, float delay)
//     {
//         Debug.Log("Waiting for animation: " + animationName);
//         yield return new WaitForSeconds(delay);
//     }

//     private void Attack()
//     {
//         Debug.Log("ATTACKING! Target: " + (target != null ? target.name : "None"));
//         animator.SetBool("Attack", true);
//         isAttacking = true;

//         if (target.CompareTag("Player"))
//         {
//             target.GetComponent<PlayerMainScript>().TakeDamage(damage);
//         }
//         else if (target.CompareTag("Tower"))
//         {
//             target.GetComponent<Tower>().TakeDamage(damage);
//         }
//         else if (target.CompareTag("Core") || isTargetingCore)
//         {
//             GameObject.FindGameObjectWithTag("Core").transform.GetComponent<Castle>().TakeDamage(damage);
//         }
//         StartCoroutine(ResetAttackBool());
//     }

//     private IEnumerator ResetAttackBool()
//     {
//         Debug.Log("Resetting attack state after delay.");
//         yield return new WaitForSeconds(1.5f);
//         animator.SetBool("Attack", false);

//         if (target || isTargetingCore)
//         {
//             isAttacking = true;
//         }
//         else
//         {
//             isAttacking = false;
//             agent.isStopped = false;
//         }
//     }

//     private void Die()
//     {
//         Debug.Log("Dying...");
//         animator.ResetTrigger("GetHit");
//         animator.SetTrigger("Die");

//         agent.isStopped = true;
//         this.enabled = false;

//         if(waveManager)
//             waveManager.UnregisterEnemy();
//         player.GetCoins(1);
//         Destroy(gameObject, 2f);
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         Debug.Log("OnTriggerEnter with: " + other.name);
//         if (other.CompareTag("Player") || other.CompareTag("Tower"))
//         {
//             target = other.transform;
//             isTargetingCore = false;
//             agent.isStopped = false;
//             Debug.Log("New target acquired: " + target.name);
//         }
//         else if (other.CompareTag("Core"))
//         {
//             Attack();
//         }
//     }

//     private void OnTriggerExit(Collider other)
//     {
//         Debug.Log("OnTriggerExit with: " + other.name);
//         // if (target == other.transform)
//         // {
//         //     SetInitialTarget();
//         //     Debug.Log("Reverting to initial target: Core");
//         // }

//         /*
//         if the enemy's target is the castle but it is near a tower, it will select the the tower as a target in probabiltiy of 0.5
//         */
//         if(isTargetingCore && other.CompareTag("Tower")){
//             float probability = UnityEngine.Random.Range(0, 1.0f);
//             if(probability < 0.5f){
//                 target = other.transform;
//                 isTargetingCore = false;
//                 isAttacking=false;
//                 Debug.Log("Updated target to tower: " + other.name);
//             }
//         }
//     }

//     void OnAnimatorMove()
//     {
//         if (animator && agent && animator.rootPosition != null && agent.nextPosition != null)
//         {
//             Vector3 position = animator.rootPosition;
//             position.y = agent.nextPosition.y;
//             transform.position = position;
//             Debug.Log("Animator moved to position: " + position);
//         }
//     }

//     private void SetInitialTarget()
//     {
//         Debug.Log("Setting initial target to Core.");
//         GameObject coreObject = GameObject.FindGameObjectWithTag("Core");
//         target = GetCoreTarget(coreObject);
//         isTargetingCore = true;
//         Debug.Log("Initial target set: " + (target != null ? target.name : "None"));
//     }

//     private Transform GetCoreTarget(GameObject coreObject)
//     {
//         if (coreObject != null)
//         {
//             BoxCollider coreCollider = coreObject.GetComponent<BoxCollider>();
//             if (coreCollider != null)
//             {
//                 Vector3 closestPoint = coreCollider.ClosestPoint(transform.position);
//                 Transform coreTarget = new GameObject("CoreTarget").transform;
//                 coreTarget.position = closestPoint;
//                 Debug.Log("Core target set to collider surface point: " + closestPoint);
//                 return coreTarget;
//             }
//             else
//             {
//                 Debug.LogWarning("Core object does not have a BoxCollider!");
//             }

//             return coreObject.transform;
//         }

//         Debug.LogError("Core object not found!");
//         return null;
//     }
// }

