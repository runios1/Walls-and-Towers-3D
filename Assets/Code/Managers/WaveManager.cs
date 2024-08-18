using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int waveNum;
    private EnemySpawner enemySpawner;
    private List<Transform[]> waveSpawnPoints;
    private int enemiesLeftForWave = 0;
    public WaveCounter waveCounter;
    public Castle castle;
    public PlayerMainScript player;
    async void Start()
    {
        waveNum = 1;
        waveCounter.ResetCounter(2);
        enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();

        castle = GameObject.FindGameObjectWithTag("Core").GetComponent<Castle>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();

        await AldenGenerator.LogAldenChat("Monsters are just beginning their way towards the castle");
        InitializeWaveSpawnPoints();
        StartNextWave();
    }

    private void InitializeWaveSpawnPoints()
    {
        waveSpawnPoints = new List<Transform[]>();

        // Wave 1
        Transform spawnPoint1 = new GameObject().transform;
        spawnPoint1.position = new Vector3(-140, 0, -10);
        waveSpawnPoints.Add(new Transform[] { spawnPoint1 });

        // Wave 2
        Transform spawnPoint2_1 = new GameObject().transform;
        spawnPoint1.position = new Vector3(-140, 0, -10);
        Transform spawnPoint2_2 = new GameObject().transform;
        spawnPoint2_2.position = new Vector3(-130, 0, -280);
        waveSpawnPoints.Add(new Transform[] { spawnPoint2_1, spawnPoint2_2 });
    }

    private async void StartNextWave()
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
            await AldenGenerator.LogAldenChat($"All the monsters are killed and you are saved by Serpina. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");

        }
    }

    public async void UnregisterEnemy()
    {
        enemiesLeftForWave--;
        if (enemiesLeftForWave == 0)
        {
            waveNum++;
            await AldenGenerator.LogAldenChat($"Wave of monsters killed. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");
            StartNextWave();
        }
    }
}
