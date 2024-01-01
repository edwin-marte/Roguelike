using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);
    public GameObject spawnMarkPrefab;
    public WaveManager waveManager;

    private List<EnemyType> enemyTypes = new List<EnemyType>();
    private Dictionary<GameObject, float> spawnTimers = new Dictionary<GameObject, float>();
    private Coroutine spawn;

    private void Start()
    {
        if (enemyTypes != null)
        {
            foreach (var enemyType in enemyTypes)
            {
                spawnTimers[enemyType.enemyPrefab] = enemyType.spawnRate - 1f;
            }
        }
    }

    private void Update()
    {
        if (waveManager == null || enemyTypes == null) return;

        if (waveManager.WaveRunning())
        {
            foreach (var enemyType in enemyTypes)
            {
                spawnTimers[enemyType.enemyPrefab] += Time.deltaTime;

                if (waveManager.GetCurrentTime() <= enemyType.startTime)
                {
                    if (spawnTimers[enemyType.enemyPrefab] >= enemyType.spawnRate)
                    {
                        StartSpawnSequence(enemyType);
                    }
                }
            }
        }
        else
        {
            if (spawn != null)
            {
                StopCoroutine(spawn);
            }
        }
    }

    public void SetSpawnRateAndAmount(GameObject enemyPrefab, float spawnRate, float startTime, int desiredAmountToSpawn, int waveIndex)
    {
        // Search for the enemy type and update it or add a new one
        EnemyType typeToUpdate = enemyTypes.Find(type => type.enemyPrefab == enemyPrefab);
        if (typeToUpdate != null)
        {
            typeToUpdate.spawnRate = spawnRate;
            typeToUpdate.startTime = startTime;
            typeToUpdate.desiredAmountToSpawn = desiredAmountToSpawn;
            typeToUpdate.waveIndex = waveIndex;
        }
        else
        {
            enemyTypes.Add(new EnemyType { enemyPrefab = enemyPrefab, spawnRate = spawnRate, startTime = startTime, desiredAmountToSpawn = desiredAmountToSpawn, waveIndex = waveIndex });
        }
    }

    private void StartSpawnSequence(EnemyType enemyType)
    {
        spawnTimers[enemyType.enemyPrefab] = 0f;
        spawn = StartCoroutine(Spawn(enemyType));
    }

    private IEnumerator Spawn(EnemyType enemyType)
    {
        float spawnAfter = 1f; // You can adjust this value as needed

        for (int i = 0; i < enemyType.desiredAmountToSpawn; i++)
        {
            GameObject spawnMark = Instantiate(spawnMarkPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            yield return new WaitForSeconds(spawnAfter);

            if (spawnMark == null) continue;

            Vector2 spawnPosition = spawnMark.transform.position;
            var enemyObj = Instantiate(enemyType.enemyPrefab, spawnPosition, Quaternion.identity);
            var enemy = enemyObj.GetComponent<Enemy>();
            float waveHealth = enemy.GetBaseHealth() + (enemy.hpWave * enemyType.waveIndex);
            enemy.SetStartingHealth(waveHealth);
            Destroy(spawnMark);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        Vector2 spawnPosition = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        return spawnPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
