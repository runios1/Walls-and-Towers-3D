using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyUnitPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    private int spawnPointWithMedkitIndex = -1;
    
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
            GameObject enemyUnit = Instantiate(enemyUnitPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
            if (spawnPointWithMedkitIndex == spawnIndex)
            {
                enemyUnit.GetComponent<EnemyUnit>().allowMedkit=true;
            }
        }
    }

    public void SetSpawnPoints(Transform[] points,bool allowMedkit = false)
    {
        spawnPoints = points;
        if(allowMedkit){
            spawnPointWithMedkitIndex = Random.Range(0, points.Length);
        }
    }
}
