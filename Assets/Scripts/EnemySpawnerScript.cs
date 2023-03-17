using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public static EnemySpawnerScript me;
    public List<GameObject> enemyPrefabs;
    public float spawnRadius;
    [Header("SPAWN AMOUNTs")]
    public int spawnAmount;
    public float spawnAmount_increaseInterval;
    private float spawnAmount_increaseTimer;
    [Header("SPAWN SPEEDs")]
    public float spawnInterval_start;
    public float spawnInterval_decreaseRate;
    private float spawnTimer;
    private void Awake()
    {
        me = this;
    }
    private void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            spawnTimer = spawnInterval_start;
            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnOne();
            }
        }
        DecreaseSpawnInterval();
        IncreaseSpawnAmount();
    }
    private void SpawnOne()
    {
        GameObject enemySpawned = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]);
        Vector3 spawnPos = RandomPointOnUnitCircle(spawnRadius);
        spawnPos += PlayerScript.me.transform.position;
        enemySpawned.transform.position = spawnPos;
    }
    public static Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, y, 0);
    }
    private void DecreaseSpawnInterval()
    {
        if (spawnInterval_start > 1)
        {
            spawnInterval_start -= spawnInterval_decreaseRate * Time.deltaTime;
        }
    }
    private void IncreaseSpawnAmount()
    {
        if (spawnAmount_increaseTimer > 0)
        {
            spawnAmount_increaseTimer -= Time.deltaTime;
        }
        else
        {
            spawnAmount_increaseTimer = spawnAmount_increaseInterval;
            spawnAmount++;
        }
    }
}
