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
    public Transform firePoint;
    public GameObject bullet;
    public GameObject towerRange;

    [Header("Tower Info")]
    public GameObject towerPrefab;
    public string towerName;
    public int cost;
    public int sellValue;
    public float health;

    [Header("Combat Stats")]
    public float minDamage;
    public float maxDamage;
    public float fireRate;
    public float range;
    [TextArea(1, 2)]
    public string description;

    float lastfired;          // The value of Time.time at the last firing moment

    // for Catcher tower 
    void Start()
    {
        sphereCol = GetComponent<SphereCollider>();
        sphereCol.radius = range;
        HideRangeUI();

        // target = null;
        bullet.SetActive(true);
    }

    #region Enemy Within Tower Range Functions

    private void OnTriggerEnter(Collider other)
    {
        if (target.CompareTag("Enemy"))
        {
            Enemy e = target.GetComponent<Enemy>();

            if (!e)
                print("GetComponent failed\n");

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
            RemoveDeadEnemies();
        }
        else { target = null; }

        TowerShooting();
    }

    private void TowerShooting()
    {
        FireTowerProjectile();
    }

    private void FireTowerProjectile()
    {
        if (Time.time - lastfired > 1 / fireRate)
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
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void RemoveDeadEnemies()
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
        towerRange.transform.localScale = Vector3.one * range * 2;
    }

    public void HideRangeUI()
    {
        towerRange.SetActive(false);
    }
}

