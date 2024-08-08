using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class TowerProjectile : MonoBehaviour
{
    [HideInInspector]
    public Tower tower;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public bool isHit;

    // public float life = 3;
    public float speed;
    public float explosionRadius = 0f;


    readonly float i = 0.05f; // delay time of bullet destruction
    Vector3 hitPosOffset = new Vector3(0, 1, 0);
    Vector3 originalTargetPos;

    private void Start()
    {
        originalTargetPos = Vector3.zero;
    }

    void Update()
    {

        if (!target || !tower)
        {
            Destroy(gameObject);
            return;
        }

        // life -= Time.deltaTime;

        // // Destroy rocket after life time is 0
        // if (life < 0) Destroy(gameObject);

        BulletMovement();
    }

    private void BulletMovement()
    {
        if (originalTargetPos == Vector3.zero)
        {
            originalTargetPos = target.position;
            transform.LookAt(target.transform);
        }
        Vector3 dir = originalTargetPos + hitPosOffset - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame && !isHit)
        {
            isHit = true;
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else { Damage(target); }

        Destroy(gameObject, i);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (target.CompareTag("Enemy"))
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        if (enemy.TryGetComponent<Enemy>(out var e))
        {

            float dmg = Random.Range(tower.minDamage, tower.maxDamage);

            // for (int i = 0; i < tD.effects.Count; i++)
            // {
            //     e.ApplyEffects(tD.effects[i]);
            // }

            e.TakeDamage(dmg);
        }
    }
}
