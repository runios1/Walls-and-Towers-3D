using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    SphereCollider sphereCol;

    // Enemy Info
    public Transform target;

    // Enemies within tower range
    private List<GameObject> _enemies = new List<GameObject>();
    private List<Enemy> _enemy = new List<Enemy>();

    [Header("Required Fields")]
    public TowerData tD;
    public Transform firePoint;
    public Transform rotatingObj;
    public GameObject bullet;
    public GameObject towerRange;

    float lastfired;          // The value of Time.time at the last firing moment

    // for Catcher tower 
    void Start()
    {
        sphereCol = GetComponent<SphereCollider>();
        sphereCol.radius = tD.range;
        HideRangeUI();

        target = null;
        bullet.SetActive(true);
    }

    #region Enemy Within Tower Range Functions

    private void OnTriggerEnter(Collider other)
    {
        if (target.CompareTag("Enemy"))
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();

            if (e.health > 0)
            {
                _enemies.Add(other.gameObject);
                _enemy.Add(e);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target.CompareTag("Enemy"))
        {
            RemoveEnemiesFromList(other);
        }
    }

    private void RemoveEnemiesFromList(Collider other)
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (other.gameObject.Equals(_enemies[i]))
            {
                _enemies.RemoveAt(i);
                _enemy.RemoveAt(i);
            }
        }
    }

    #endregion

    void Update()
    {
        if (_enemies.Count > 0)
        {
            target = _enemies[0].transform;
            RemoveDeadOrPoisonedEnemies();
        }
        else { target = null; }

        if (target != null)
            RotateTowerTowardsTarget();

        TowerShooting();
    }

    private void TowerShooting()
    {
        FireTowerProjectile();
    }

    private void FireTowerProjectile()
    {
        if (Time.time - lastfired > 1 / tD.fireRate)
        {
            lastfired = Time.time;

            GameObject b = Instantiate(bullet, firePoint.position, bullet.transform.rotation);
            TowerProjectile _twrBullet = b.GetComponent<TowerProjectile>();
            _twrBullet.isHit = false;

            if (_twrBullet != null)
            {
                _twrBullet.target = target;
                _twrBullet.tower = this;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        tD.health -= damage;
        if (tD.health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void RotateTowerTowardsTarget()
    {
        Vector3 dir = target.transform.position - rotatingObj.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        rotatingObj.transform.rotation = Quaternion.Slerp(rotatingObj.transform.rotation, rot, 10f * Time.deltaTime);
    }

    private void RemoveDeadOrPoisonedEnemies()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemy[i].health <= 0)
            {
                _enemies.RemoveAt(i);
                _enemy.RemoveAt(i);
            }
        }
    }

    public void ShowRangeUI()
    {
        towerRange.SetActive(true);
        towerRange.transform.localScale = Vector3.one * tD.range * 2;
    }

    public void HideRangeUI()
    {
        towerRange.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, tD.range);
    }
}

