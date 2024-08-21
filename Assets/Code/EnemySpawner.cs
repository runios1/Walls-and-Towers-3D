using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;
    private GameObject[] enemies = new GameObject[] { };

    public void StartSpawn(int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Invoke(nameof(SpawnEnemy), 3f + i * spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        enemies = enemies.Append(newEnemy).ToArray();  // Update the array with the new sequence
    }

    public void SetSpawnPoints(Transform[] points)
    {
        spawnPoints = points;
    }

    // public bool NoEnemiesLeft()
    // {
    //     return enemies != null && enemies.All(enemy => enemy == null || enemy.IsDestroyed());
    // }
    public bool NoEnemiesLeft()
    {
        // Return false if no enemies have been spawned yet
        if (enemies.Count() == 0) return false;

        // Check if all enemies are null or destroyed
        return enemies.All(enemy => enemy == null || enemy.IsDestroyed());
    }
}
