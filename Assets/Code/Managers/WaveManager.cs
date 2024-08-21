using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private int waveNum;
    private EnemySpawner enemySpawner;
    private List<Transform[]> waveSpawnPoints;
    //private int enemiesLeftForWave = 0;
    public WaveCounter waveCounter;
    public Castle castle;
    public PlayerMainScript player;
    private bool gameOver = false;
    async void Start()
    {
        waveNum = -1;
        waveCounter.ResetCounter(2);
        enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();

        castle = GameObject.FindGameObjectWithTag("Core").GetComponent<Castle>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();

        await AldenGenerator.LogAldenChat("Monsters are just beginning their way towards the castle");
        InitializeWaveSpawnPoints();
        // StartNextWave();
    }

    async void Update(){
        if(waveSpawnPoints != null){
            Debug.Log("waveNum: " + waveNum + "NoEnemies left: " + enemySpawner.NoEnemiesLeft());
            if(waveNum == -1){
                waveNum++;
                await AldenGenerator.LogAldenChat("Monsters are approaching the castle");
                Debug.Log("Wave " + waveNum + " started");
                StartNextWave();
            }else if(enemySpawner.NoEnemiesLeft()){
                Debug.Log("Wave " + waveNum + " completed");
                await AldenGenerator.LogAldenChat($"Wave of monsters killed. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");
                waveNum++;
                StartNextWave();
            }else if(gameOver){
                await AldenGenerator.LogAldenChat($"All the monsters are killed and you are saved by Serpina. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");
            }
        }
    }

    private void InitializeWaveSpawnPoints()
    {
        waveSpawnPoints = new List<Transform[]>();

        // Wave 1
        Transform spawnPoint1 = new GameObject().transform;
        spawnPoint1.position = new Vector3(-140, 0, -10);
        //create more spawn points by adding noise to spawnPoint1's position
        Vector3 spawnPoint2_pos = spawnPoint1.position + new Vector3(Random.Range(-20, 70), 0, 0);
        Transform spawnPoint2 = new GameObject().transform;
        spawnPoint2.position = spawnPoint2_pos;

        waveSpawnPoints.Add(new Transform[] { spawnPoint1, spawnPoint2 });

        // Wave 2
        Transform spawnPoint2_1 = new GameObject().transform;
        spawnPoint1.position = new Vector3(-140, 0, -10);
        Transform spawnPoint2_2 = new GameObject().transform;
        spawnPoint2_2.position = new Vector3(-130, 0, -280);
        waveSpawnPoints.Add(new Transform[] { spawnPoint2_1, spawnPoint2_2 });
    }

    private void StartNextWave()
    {
        // if (waveNum - 1 < waveSpawnPoints.Count)
        // {
        //     Debug.Log("Wave" + waveNum);
        //     waveCounter.IncreaseCounter();
        //     enemySpawner.SetSpawnPoints(waveSpawnPoints[waveNum - 1]);
        //     enemiesLeftForWave = 5;
        //     enemySpawner.StartSpawn(enemiesLeftForWave);
        // }
        // else
        // {
        //     Debug.Log("All waves completed!");
        //     await AldenGenerator.LogAldenChat($"All the monsters are killed and you are saved by Serpina. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");

        // }
        Debug.LogWarning("waveNum: " + waveNum + ", waveSpawnPoints.Count: " + waveSpawnPoints.Count);
        if(waveNum < waveSpawnPoints.Count){
            Debug.LogWarning("Wave" + (waveNum+1));
            waveCounter.IncreaseCounter();
            enemySpawner.SetSpawnPoints(waveSpawnPoints[waveNum]);
            Debug.LogWarning("waveSpawnPoints[waveNum].Count(): " + waveSpawnPoints[waveNum].Count());
            enemySpawner.StartSpawn(waveSpawnPoints[waveNum].Count());
        }else{
            Debug.Log("All waves completed!");
            gameOver = true;
        }

    }

    // public async void UnregisterEnemy()
    // {
    //     enemiesLeftForWave--;
    //     if (enemiesLeftForWave == 0)
    //     {
    //         waveNum++;
    //         await AldenGenerator.LogAldenChat($"Wave of monsters killed. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");
    //         StartNextWave();
    //     }
    // }
}
