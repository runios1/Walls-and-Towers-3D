using System.Collections.Generic;
using System.Linq;
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

    public GameObject victoryMenu;

    public AldenGenerator aldenGenerator;

    public bool allowMedKit = false;
    public int allowMedkitFromWave = 3;


    public GameObject miniboss;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        enemiesLeftForWave = new int[5];
        waveNum = 1;
        waveCounter.ResetCounter(5);
        enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();

        castle = GameObject.FindGameObjectWithTag("Core").GetComponent<Castle>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMainScript>();

        aldenGenerator.LogAldenChat("Monsters are just beginning their way towards the castle");
        InitializeWaveSpawnPoints2();
        StartNextWave();
    }

    // private void InitializeWaveSpawnPoints()
    // {
    //     waveSpawnPoints = new List<Transform[]>();

    //     // Wave 1
    //     Transform spawnPoint1 = new GameObject().transform;
    //     spawnPoint1.position = new Vector3(-140, 0, -10);
    //     waveSpawnPoints.Add(new Transform[] { spawnPoint1 });
    //     enemiesLeftForWave[0] = 5;

    //     // Wave 2
    //     Transform spawnPoint2_1 = new GameObject().transform;
    //     spawnPoint1.position = new Vector3(-140, 0, -10);
    //     Transform spawnPoint2_2 = new GameObject().transform;
    //     spawnPoint2_2.position = new Vector3(-130, 0, -280);
    //     waveSpawnPoints.Add(new Transform[] { spawnPoint2_1, spawnPoint2_2 });
    //     enemiesLeftForWave[1] = 10;

    // }
    private void InitializeWaveSpawnPoints2()
    {
        waveSpawnPoints = new List<Transform[]>();
        Vector3[] interestPoints = new Vector3[]
        {
        new Vector3(-140, 0, -10),
        new Vector3(-140, 0, -280),
        new Vector3(-47.7000008f,0,-138.100006f),
        new Vector3(-258.700012f,0,-138.100006f)
        };

        // Wave 1
        Transform spawnPoint1 = new GameObject().transform;
        spawnPoint1.position = interestPoints[0];
        waveSpawnPoints.Add(new Transform[] { spawnPoint1 });
        enemiesLeftForWave[0] = 5;

        // Wave 2
        Transform spawnPoint2_1 = new GameObject().transform;
        Transform spawnPoint2_2 = new GameObject().transform;
        int[] selectedIndices = Enumerable.Range(0, interestPoints.Length).OrderBy(x => Random.value).Take(2).ToArray();
        spawnPoint2_1.position = interestPoints[selectedIndices[0]];
        spawnPoint2_2.position = interestPoints[selectedIndices[1]];
        waveSpawnPoints.Add(new Transform[] { spawnPoint2_1, spawnPoint2_2 });
        enemiesLeftForWave[1] = 10;

        // Wave 3 (Adding noise)
        Transform spawnPoint3_1 = new GameObject().transform;
        Transform spawnPoint3_2 = new GameObject().transform;
        Transform spawnPoint3_3 = new GameObject().transform;
        Transform spawnPoint3_4 = new GameObject().transform;
        spawnPoint3_1.position = AddNoise(interestPoints[selectedIndices[0]]);
        spawnPoint3_2.position = AddNoise(interestPoints[selectedIndices[1]]);
        selectedIndices = Enumerable.Range(0, interestPoints.Length).OrderBy(x => Random.value).Take(2).ToArray();
        spawnPoint3_3.position = interestPoints[selectedIndices[0]];
        spawnPoint3_4.position = interestPoints[selectedIndices[1]];
        waveSpawnPoints.Add(new Transform[] { spawnPoint3_1, spawnPoint3_2, spawnPoint3_3, spawnPoint3_4 });
        enemiesLeftForWave[2] = 20;  // Adjust the number as needed


        // Wave 4
        Transform spawnPoint4_1 = new GameObject().transform;
        Transform spawnPoint4_2 = new GameObject().transform;
        Transform spawnPoint4_3 = new GameObject().transform;
        Transform spawnPoint4_4 = new GameObject().transform;
        spawnPoint4_1.position = interestPoints[0];
        spawnPoint4_2.position = interestPoints[1];
        spawnPoint4_3.position = interestPoints[2];
        spawnPoint4_4.position = interestPoints[3];
        waveSpawnPoints.Add(new Transform[] { spawnPoint4_1, spawnPoint4_2, spawnPoint4_3, spawnPoint4_4 });
        enemiesLeftForWave[3] = 40;


        // Wave 5 - Boss wave
        Transform spawnPoint5_1 = new GameObject().transform;
        Transform spawnPoint5_2 = new GameObject().transform;
        Transform spawnPoint5_3 = new GameObject().transform;
        Transform spawnPoint5_4 = new GameObject().transform;
        spawnPoint5_1.position = interestPoints[0];
        spawnPoint5_2.position = interestPoints[1];
        spawnPoint5_3.position = interestPoints[2];
        spawnPoint5_4.position = interestPoints[3];
        waveSpawnPoints.Add(new Transform[] { spawnPoint5_1, spawnPoint5_2, spawnPoint5_3, spawnPoint5_4 });
        enemiesLeftForWave[4] = 40;
    }
    // private void InitializeWaveSpawnPoints3()
    // {
    //     waveSpawnPoints = new List<Transform[]>();
    //     Vector3[] interestPoints = new Vector3[]
    //     {
    //     new Vector3(-140, 0, -10),
    //     new Vector3(-140, 0, -280),
    //     new Vector3(-47.7000008f,0,-138.100006f),
    //     new Vector3(-258.700012f,0,-138.100006f)
    //     };

    //     int[] selectedIndices = Enumerable.Range(0, interestPoints.Length).OrderBy(x => Random.value).Take(2).ToArray();


    //     // Wave 3 (Adding noise)
    //     Transform spawnPoint3_1 = new GameObject().transform;
    //     Transform spawnPoint3_2 = new GameObject().transform;
    //     Transform spawnPoint3_3 = new GameObject().transform;
    //     Transform spawnPoint3_4 = new GameObject().transform;
    //     spawnPoint3_1.position = AddNoise(interestPoints[selectedIndices[0]]);
    //     spawnPoint3_2.position = AddNoise(interestPoints[selectedIndices[1]]);
    //     selectedIndices = Enumerable.Range(0, interestPoints.Length).OrderBy(x => Random.value).Take(2).ToArray();
    //     spawnPoint3_3.position = interestPoints[selectedIndices[0]];
    //     spawnPoint3_4.position = interestPoints[selectedIndices[1]];
    //     miniboss.transform.position = spawnPoint3_1.position;
    //     waveSpawnPoints.Add(new Transform[] { spawnPoint3_1 });

    //     enemiesLeftForWave[0] = 5;  // Adjust the number as needed

    // }
    private Vector3 AddNoise(Vector3 position)
    {
        float noiseX = Random.Range(-30f, 20f);  // Adjust noise level as needed
        float noiseZ = Random.Range(-30f, 20f);
        return new Vector3(position.x + noiseX, position.y, position.z + noiseZ);
    }


    private void StartNextWave()
    {
        if (waveNum - 1 < waveSpawnPoints.Count)
        {
            Debug.Log("Wave" + waveNum);
            waveCounter.IncreaseCounter();
            enemySpawner.SetSpawnPoints(waveSpawnPoints[waveNum - 1], allowMedKit && waveNum >= allowMedkitFromWave);

            // Set the isFirstWave flag for the first wave
            if (waveNum == 1)
            {
                enemySpawner.enemyUnitPrefab.GetComponent<EnemyUnit>().isFirstWave = true;
            }
            else
            {
                enemySpawner.enemyUnitPrefab.GetComponent<EnemyUnit>().isFirstWave = false;
            }

            enemySpawner.StartSpawn((enemiesLeftForWave[waveNum - 1] / 5) / waveSpawnPoints[waveNum - 1].Length); // StartSpawn spawns 5 enemies at a time as a unit, each spawn point gets the same portion of enemies

            if (waveNum == waveSpawnPoints.Count)
            {
                enemiesLeftForWave[waveNum - 1]++;
                Instantiate(miniboss, miniboss.transform.position, miniboss.transform.rotation);
            }
        }
        else
        {
            Debug.Log("All waves completed!");
            aldenGenerator.LogAldenChat($"All the monsters are killed and you are saved by Serpina. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");

            audioSource.clip = victorySound;
            audioSource.Play();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            victoryMenu.SetActive(true);
            //victoryMenu.GetComponentInParent<GameOverMenu>().LoadMenu();
        }
    }

    public void UnregisterEnemy()
    {
        enemiesLeftForWave[waveNum - 1]--;
        Debug.Log("Enemies left for wave: " + waveNum + ": " + enemiesLeftForWave[waveNum - 1]);
        if (enemiesLeftForWave[waveNum - 1] == 0)
        {
            waveNum++;

            if (allowMedKit && waveNum == allowMedkitFromWave)
                aldenGenerator.LogAldenChat("Monsters are approaching the castle. The real game starts now. Tell Serpina that enemies can drop Medkits from now on.");
            else
                aldenGenerator.LogAldenChat($"Wave of monsters killed. Serpina's health is {player.health}/100, castle's health is {castle.health}/100");

            audioSource.clip = waveCompleteSound;
            audioSource.Play();

            StartNextWave();
        }
    }



}