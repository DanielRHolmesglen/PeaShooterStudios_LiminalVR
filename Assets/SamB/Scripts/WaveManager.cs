using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;
    public int totalWaves = 5;
    public int enemiesPerWave = 10;
    public float timeBetweenSpawns = 2.0f;
    public float timeBetweenWaves = 5.0f;

    private int currentWave = 1;

  
    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

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

        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        //instantiating a new enemy (Random enemy, at position of a random spawn point, default/no rotation)
        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnIndex].transform.position, Quaternion.identity);
    }

}
