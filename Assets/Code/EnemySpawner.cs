using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;

    public void StartSpawn(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Invoke(nameof(SpawnEnemy), 2f + i * spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }

    public void SetSpawnPoints(Transform[] points)
    {
        spawnPoints = points;
    }
}
