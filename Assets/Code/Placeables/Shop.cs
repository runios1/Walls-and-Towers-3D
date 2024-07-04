using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private PlayerMainScript player;
    private Dictionary<string, Placeable> itemPrefabs;


    [Header("Placeable Prefabs")]
    [SerializeField] private Placeable towerPrefab;
    [SerializeField] private Placeable wallPrefab;
    void Start()
    {
        itemPrefabs = new Dictionary<string, Placeable>()
        {
            { "Tower", towerPrefab },
            {"Wall", wallPrefab},
        };
    }

    public void Buy(string itemID)
    {
        if (itemPrefabs.TryGetValue(itemID, out Placeable itemPrefab))
        {
            if (player.LoseCoins(itemPrefab.cost))
                player.PlaceItem(itemPrefab);
        }
        else
            Debug.Log($"Item {itemID} not found");

    }

}
