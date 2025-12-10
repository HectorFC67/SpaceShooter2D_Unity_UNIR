using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float maxSpawnTime = 2f;

    float timer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnEnemy()
    {
        Camera cam = Camera.main;
        float yExtent = cam.orthographicSize;
        float xRight = cam.ViewportToWorldPoint(new Vector3(1.1f, 0f, 0f)).x;

        float randomY = Random.Range(-yExtent, yExtent);

        Vector3 spawnPos = new Vector3(xRight, randomY, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}