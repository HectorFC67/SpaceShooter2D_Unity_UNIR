using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject shooterEnemyPrefab;

    [Header("Spawn Time Base")]
    [SerializeField] float baseMinSpawnTime = 0.5f;
    [SerializeField] float baseMaxSpawnTime = 2f;

    [Header("Spawn Time Difficulty Scaling")]
    [SerializeField] int scorePerDifficultyStep = 60;      // cada 60 puntos, más difícil
    [SerializeField] float spawnTimeReductionPerStep = 0.1f;

    [SerializeField] float minSpawnTimeClamp = 0.15f;
    [SerializeField] float maxSpawnTimeClamp = 0.7f;

    [Header("Shooter Enemy Unlock")]
    [SerializeField] int scoreToUnlockShooter = 150;
    [SerializeField, Range(0f, 1f)] float shooterChance = 0.3f; // 30% shooter, 70% normal

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
        int score = 0;
        if (GameManager.Instance != null)
        {
            score = GameManager.Instance.CurrentScore;
        }

        int steps = Mathf.FloorToInt(score / scorePerDifficultyStep);

        float minTime = baseMinSpawnTime - steps * spawnTimeReductionPerStep;
        float maxTime = baseMaxSpawnTime - steps * spawnTimeReductionPerStep;

        minTime = Mathf.Clamp(minTime, minSpawnTimeClamp, baseMinSpawnTime);
        maxTime = Mathf.Clamp(maxTime, maxSpawnTimeClamp, baseMaxSpawnTime);

        timer = Random.Range(minTime, maxTime);
    }

    void SpawnEnemy()
    {
        Camera cam = Camera.main;
        float yExtent = cam.orthographicSize;
        float xRight = cam.ViewportToWorldPoint(new Vector3(1.1f, 0f, 0f)).x;

        float randomY = Random.Range(-yExtent, yExtent);
        Vector3 spawnPos = new Vector3(xRight, randomY, 0f);

        int score = 0;
        if (GameManager.Instance != null)
        {
            score = GameManager.Instance.CurrentScore;
        }

        GameObject prefabToSpawn = enemyPrefab;

        // Solo puede aparecer el shooter si el score >= 150
        if (score >= scoreToUnlockShooter && shooterEnemyPrefab != null)
        {
            if (Random.value < shooterChance)
            {
                prefabToSpawn = shooterEnemyPrefab;
            }
        }

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}
