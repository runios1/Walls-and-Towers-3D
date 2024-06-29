using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Towers")]
public class TowerData : ScriptableObject
{
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
    [Space]
    public TowerData upgrade01;
    public TowerData upgrade02;
}

