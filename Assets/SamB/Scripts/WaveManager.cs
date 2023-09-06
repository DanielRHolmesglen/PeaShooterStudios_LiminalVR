using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


  
    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    //spawn waves, with set wait times between each spawn/wave
    private IEnumerator SpawnWaves()
    {
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

        //randomly picking enemy prefab and spawn point. Will updte this to also add a prefab to the list, so each wave is a new enemy type. 
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        //instantiating a new enemy (Random enemy, at position of a random spawn point, default/no rotation)

        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnIndex].transform.position, Quaternion.identity);
    }

}
