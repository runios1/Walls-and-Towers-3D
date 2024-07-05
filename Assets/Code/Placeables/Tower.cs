using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : Placeable
{

    // Enemy Info
    public Transform target;

    // Enemies within tower range
    private List<GameObject> _enemies = new List<GameObject>();

    [Header("Required Fields")]
    public Transform firePoint;
    public GameObject bullet;
    // public GameObject towerRange;



    [Header("Combat Stats")]
    public float minDamage;
    public float maxDamage;
    public float fireRate;

    float lastfired;          // The value of Time.time at the last firing moment

    public override void Start()
    {
        base.Start();
        target = null;
        bullet.SetActive(true);
    }

    #region Enemy Within Tower Range Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            _enemies.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RemoveEnemiesFromList(other);
        }
        else if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
        }
    }

    private void RemoveEnemiesFromList(Collider other)
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (other.gameObject.Equals(_enemies[i]))
            {
                _enemies.RemoveAt(i);
            }
        }
    }

    #endregion

    void Update()
    {
        RemoveDeadEnemies();

        if (_enemies.Count > 0)
        {
            target = _enemies[0].transform;
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



    private void RemoveDeadEnemies()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] == null)
                _enemies.RemoveAt(i);

        }
    }

    // public void ShowRangeUI()
    // {
    //     towerRange.SetActive(true);
    //     towerRange.transform.localScale = Vector3.one * range * 2;
    // }

    // public void HideRangeUI()
    // {
    //     towerRange.SetActive(false);
    // }
}

