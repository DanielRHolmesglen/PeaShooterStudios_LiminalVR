using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Responsible for the Managing wave spawning and tracks all enemies in scene. There is no list for bugs spawned at current as I dont think it's needed, but can add it.
/// </summary>
/// 

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnPoints;
    public int totalWaves = 10;
    public int enemiesPerWave = 12; //also increases by 4 each wave.
    public int extraEnemiesPerWave = 2;
    public float timeBetweenSpawns = 1.0f;
    public float timeBetweenWaves = 15.0f;

    public static List<GameObject> currentEnemies = new List<GameObject>(); 
    public Text currentWaveText;
    public Text enemiesLeftText;

    public int previousWave = 0;
    public int currentWave = 1;

    private NavMeshSurface navMeshSurface;
    public ShipManager shipManager; //used for victory 

    private void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
        {
            Debug.LogError("No NavMeshSurface found in the scene. Make sure you have one for navigation.");
            return;
        }

    }

    private void FixedUpdate()
    {
        enemiesLeftText.text = currentEnemies.Count.ToString(); //updating the wave tracker when things die 
        //currentEnemies.RemoveAll(s => s == null); //making sure the list properly empties
    }

    //spawn waves, with set wait times between each spawn/wave
    public IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2.0f);
        //int currentWave = 0;

        
        for (; currentWave <= totalWaves; currentWave++)
        {

            if (currentWave == totalWaves) shipManager.OnVictory();

            currentWaveText.text = currentWave + "/" + totalWaves; //updating UI

            // Resetting the list of enemies
            currentEnemies.Clear();

            
            
            enemiesPerWave = enemiesPerWave + extraEnemiesPerWave;


            // Add new prefabs to "spawnPool"
            int prefabIndex = Mathf.Min(currentWave - 1, enemyPrefabs.Length - 1);

            //picking a random enemy from spawn pool to spawn
            int randomIndex;
            GameObject selectedPrefab;
           
            for (int enemyIndex = 0; enemyIndex < enemiesPerWave; enemyIndex++) //going through and spawning enemies until index is full
            {
                randomIndex = Random.Range(0, currentWave - 1) % (enemyPrefabs.Length - 1);
                selectedPrefab = enemyPrefabs[randomIndex];
                SpawnRandomEnemy(selectedPrefab);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            //wait for enemies to be defeated.
            while (currentEnemies.Count > 0)
            {
                yield return null;
            }

            Debug.LogWarning("wave" + currentWave);
           

            yield return new WaitForSeconds(1);
        }



    }

    private void SpawnRandomEnemy(GameObject prefab)
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
        GameObject newEnemy = Instantiate(prefab, spawnPosition, spawnRotation);
        currentEnemies.Add(newEnemy); //adding enemy to list for UI
        enemiesLeftText.text = currentEnemies.Count.ToString(); //updating with how many enemies are now in the enemy array

    }


    public IEnumerator DestroyEnemies()
    {
        foreach (GameObject enemy in currentEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
            
            yield return new WaitForSeconds(0.1f);
        }

        // Clear the list after destroying the enemies
        currentEnemies.Clear();

        yield return null;
    }


}
