using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;      
    public Transform[] spawnPoints;

    public float startSpawnInterval = 4f;
    public float minSpawnInterval = 1.5f;
    public float intervalDecreaseRate = 0.1f;
    public float decreaseIntervalEvery = 5f;

    private float currentSpawnInterval;
    private float decreaseTimer;

    void Start()
    {
        currentSpawnInterval = startSpawnInterval;
        decreaseTimer = decreaseIntervalEvery;
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        decreaseTimer -= Time.deltaTime;
        if (decreaseTimer <= 0f)
        {
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalDecreaseRate);
            decreaseTimer = decreaseIntervalEvery;
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject chosenEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        Instantiate(chosenEnemy, spawnPoint.position, Quaternion.identity);
    }
}
