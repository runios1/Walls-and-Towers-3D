using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public GameObject magicCirclePrefab;
    public GameObject[] enemyPrefabs;
    private List<Transform> locations;
    private int enemyCount;
    public bool allowMedkit = false;
    public bool isFirstWave = false; // Flag to indicate the first wave

    // Start is called before the first frame update
    void Start()
    {
        //enemies = gameObject.GetComponentsInChildren<BaseEnemy>();
        locations = new List<Transform>(gameObject.GetComponentsInChildren<Transform>().Where(t => t.name.StartsWith("Location")));
        String locationsString = string.Join(", ", locations.Select(t => t.position).ToArray());
        //Debug.Log(locationsString);
        enemyCount = locations.Count;
        magicCirclePrefab.SetActive(true);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        int enemyIndexWithMedkit = -1;
        if (allowMedkit)
        {
            enemyIndexWithMedkit = UnityEngine.Random.Range(0, locations.Count);
        }

        foreach (Transform spawnPosition in locations)
        {
            GameObject enemyPrefab;

            if (isFirstWave)
            {
                // Spawn only weak enemies for the first wave
                enemyPrefab = enemyPrefabs[0];
            }
            else
            {
                // Randomly select an enemy prefab for each spawn position
                enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition.position, spawnPosition.rotation, gameObject.transform);
            if (enemyIndexWithMedkit == locations.IndexOf(spawnPosition))
            {
                enemy.GetComponent<BaseEnemy>().allowMedkit = true;
            }
            //Debug.Log($"Spawning enemy {enemy.name} at {spawnPosition.position}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (magicCirclePrefab != null && magicCirclePrefab.GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(magicCirclePrefab);
        }
    }

    public void UnregisterEnemy()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            Destroy(gameObject);
        }
    }
}