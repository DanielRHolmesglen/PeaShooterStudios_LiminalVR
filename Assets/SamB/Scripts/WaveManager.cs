using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Responsible for the Managing wave spawning. There is no list for bugs spawned at current as I dont think it's needed, but can add it.
/// </summary>
public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;
    public int totalWaves = 5;
    public int enemiesPerWave = 10;
    public float timeBetweenSpawns = 2.0f;
    public float timeBetweenWaves = 5.0f;

    private NavMeshSurface navMeshSurface;


    private void Start()
    {
        
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("No NavMeshSurface found in the scene. Make sure you have one for navigation.");
            return;
        }
        

        StartCoroutine(SpawnWaves());
    }

    //spawn waves, with set wait times between each spawn/wave
    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2.0f); // Wait for 2 seconds before starting to ensure NavMesh is built

        for (int wave = 1; wave <= totalWaves; wave++)
        {
            for (int enemyIndex = 0; enemyIndex < enemiesPerWave; enemyIndex++)
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
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned.");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
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
        navMeshSurface.BuildNavMesh(); // Rebuild NavMesh after spawning an enemy

    }

}
