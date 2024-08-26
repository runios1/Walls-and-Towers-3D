using System.Collections.Generic;
using UnityEngine;
// using static AldenGenerator;

public class WaveManager : MonoBehaviour
{
    private int waveNum;
    private EnemySpawner enemySpawner;
    private List<Transform[]> waveSpawnPoints;
    private int[] enemiesLeftForWave; // Value of enemiesLeftForWave[i] must be divisible by 5 * waveSpawnPoints[i] without a remainder
    public WaveCounter waveCounter;
    public Castle castle;
    public PlayerMainScript player;
    [Header("Audio")]
    private AudioSource audioSource;

    public AudioClip waveCompleteSound;
    public AudioClip victorySound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        enemiesLeftForWave = new int[2];
        waveNum = 1;
        waveCounter.ResetCounter(2);
        enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();

        castle = GameObject.FindGameObjectWithTag("Core").GetComponent<Castle>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();

        // await LogAldenChat("Monsters are just beginning their way towards the castle");
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
        enemiesLeftForWave[0] = 5;

        // Wave 2
        Transform spawnPoint2_1 = new GameObject().transform;
        spawnPoint1.position = new Vector3(-140, 0, -10);
        Transform spawnPoint2_2 = new GameObject().transform;
        spawnPoint2_2.position = new Vector3(-130, 0, -280);
        waveSpawnPoints.Add(new Transform[] { spawnPoint2_1, spawnPoint2_2 });
        enemiesLeftForWave[1] = 10;

    }

    private void StartNextWave()
    {
        if (waveNum - 1 < waveSpawnPoints.Count)
        {
            Debug.Log("Wave" + waveNum);
            waveCounter.IncreaseCounter();
            enemySpawner.SetSpawnPoints(waveSpawnPoints[waveNum - 1]);
            enemySpawner.StartSpawn((enemiesLeftForWave[waveNum - 1] / 5) / waveSpawnPoints[waveNum - 1].Length); // StartSpawn spawns 5 enemies at a time as a unit, each spawn point gets the same portion of enemies
        }
        else
        {
            Debug.Log("All waves completed!");
            // await LogAldenChat($"All the monsters are killed and you are saved by Serpina. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");

            audioSource.clip = victorySound;
            audioSource.Play();
        }
    }

    public void UnregisterEnemy()
    {
        enemiesLeftForWave[waveNum - 1]--;
        if (enemiesLeftForWave[waveNum - 1] == 0)
        {
            waveNum++;
            // await LogAldenChat($"Wave of monsters killed. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");

            audioSource.clip = waveCompleteSound;
            audioSource.Play();

            StartNextWave();
        }
    }
}