using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Responsible for the Managing wave spawning. There is no list for bugs spawned at current as I dont think it's needed, but can add it.
/// </summary>
public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;
    public int totalWaves = 5;
    public int enemiesPerWave = 10; //increases by 2 each wave.
    public float timeBetweenSpawns = 2.0f;
    public float timeBetweenWaves = 5.0f;

    public List<GameObject> currentEnemies /*saying the list exists*/ = new List<GameObject>(); //actually creating the list
    public Text currentWaveText;
    public Text enemiesLeftText;

    public int currentWave = 0;


    private NavMeshSurface navMeshSurface;


    private void Start()
    {
        
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("No NavMeshSurface found in the scene. Make sure you have one for navigation.");
            return;
        }

        //StartCoroutine(SpawnWaves());
        //This is no longer done by wave manager, instead is done by StartingSequence so it happens at the correct time.
    }

    //spawn waves, with set wait times between each spawn/wave
    public IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2.0f); // Wait for 2 seconds before starting cause its nice
        int wave = 0;

        
            for (; wave <= totalWaves; wave++)
            {
                currentWave++; //for UI
                currentWaveText.text = currentWave + "/5"; //updating UI
                enemiesPerWave = enemiesPerWave + 2; //increasing amount of enemies each wave by 2
                Debug.LogWarning("wave" + wave);


                for (int enemyIndex = 0; enemyIndex < enemiesPerWave; enemyIndex++) //going through and spawning enemies until index is full
                {
                    SpawnRandomEnemy();
                    yield return new WaitForSeconds(timeBetweenSpawns);
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }

        
        
    }

    private void SpawnRandomEnemy()
    {

        //debugging in case lists are empty (they shouldnt be)
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs and/or spawn points assigned.");
            return;
        }

        // Randomly picking enemy prefab and spawn point.
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        // Get the height offset from the enemy prefab
        float heightOffset = enemyPrefabs[randomEnemyIndex].transform.position.y;


        // Get the spawn position and rotation
        Vector3 spawnPosition = spawnPoints[randomSpawnIndex].transform.position;
        spawnPosition.y += heightOffset;
        Quaternion spawnRotation = Quaternion.identity;

        // Sample a valid position on the NavMesh near the spawn point
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPosition, out hit, 5f, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }

        // Spawning the enemy
        GameObject newEnemy = Instantiate(enemyPrefabs[randomEnemyIndex], spawnPosition, spawnRotation);
        currentEnemies.Add(newEnemy); //adding enemy to list for UI

        enemiesLeftText.text = currentEnemies.Count.ToString(); //updating  with how many enemies are now in the enemy array
        //navMeshSurface.BuildNavMesh(); //Can't be done during runtime/isnt needed. Dunno why it was suggested.

    }

}
