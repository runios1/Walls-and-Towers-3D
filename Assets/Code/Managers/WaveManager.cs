using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int waveNum;
    private EnemySpawner enemySpawner;
    private List<Transform[]> waveSpawnPoints;
    private int enemiesLeftForWave = 0;
    public WaveCounter waveCounter;
    // Start is called before the first frame update
    void Start()
    {
        waveNum = 1;
        waveCounter.ResetCounter(2);
        enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
        InitializeWaveSpawnPoints();
        StartNextWave();
    }

    private void InitializeWaveSpawnPoints()
    {
        waveSpawnPoints = new List<Transform[]>();

        // Wave 1
        Transform spawnPoint1 = new GameObject().transform;
        spawnPoint1.position = new Vector3(-150, 0, -100);
        waveSpawnPoints.Add(new Transform[] { spawnPoint1 });

        // Wave 2
        Transform spawnPoint2_1 = new GameObject().transform;
        spawnPoint2_1.position = new Vector3(-150, 0, -100);
        Transform spawnPoint2_2 = new GameObject().transform;
        spawnPoint2_2.position = new Vector3(-150, 0, -200);
        waveSpawnPoints.Add(new Transform[] { spawnPoint2_1, spawnPoint2_2 });
    }

    private void StartNextWave()
    {
        if (waveNum - 1 < waveSpawnPoints.Count)
        {
            Debug.Log("Wave" + waveNum);
            waveCounter.IncreaseCounter();
            enemySpawner.SetSpawnPoints(waveSpawnPoints[waveNum - 1]);
            enemiesLeftForWave = 5;
            enemySpawner.StartSpawn(enemiesLeftForWave);
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    public void UnregisterEnemy()
    {
        enemiesLeftForWave--;
        if (enemiesLeftForWave == 0)
        {
            waveNum++;
            StartNextWave();
        }
    }
}
