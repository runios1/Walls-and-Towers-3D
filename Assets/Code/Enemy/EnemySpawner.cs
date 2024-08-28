using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyUnitPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    
    public void StartSpawn(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Invoke(nameof(SpawnEnemy), 3f + i * spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        for (int spawnIndex = 0; spawnIndex < spawnPoints.Length; spawnIndex++){
            Instantiate(enemyUnitPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        }
    }

    public void SetSpawnPoints(Transform[] points)
    {
        spawnPoints = points;
    }
}
