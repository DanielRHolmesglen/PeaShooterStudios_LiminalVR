using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform spawnPoint;
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

        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, Quaternion.identity);
    }
}
