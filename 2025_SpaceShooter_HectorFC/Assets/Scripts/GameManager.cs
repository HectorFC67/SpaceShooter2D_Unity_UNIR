using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] int pointsPerSecond = 10;
    [SerializeField] int scoreKill = 25;

    int score = 0;
    float scoreTimeAccumulator = 0f;

    [Header("Lives")]
    [SerializeField] List<GameObject> heartIcons;
    [SerializeField] int maxLives = 3;

    int currentLives;
    PlayerSpaceShip player;

    [Header("Game Over")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] BGScroll[] bgScrolls;
    [SerializeField] EnemySpawner[] enemySpawners;

    bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        player = FindObjectOfType<PlayerSpaceShip>();

        currentLives = maxLives;
        UpdateHearts();
        UpdateScoreText();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;

        scoreTimeAccumulator += Time.deltaTime * pointsPerSecond;

        if (scoreTimeAccumulator >= 1f)
        {
            int increment = Mathf.FloorToInt(scoreTimeAccumulator);
            score += increment;
            scoreTimeAccumulator -= increment;
            UpdateScoreText();
        }
    }

    // Score

    public void AddScore(int amount)
    {
        if (amount <= 0) return;

        score += amount;
        UpdateScoreText();
    }

    public void OnEnemyKilled()
    {
        if (isGameOver) return;

        AddScore(scoreKill);
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    // Lives

    public void OnPlayerHit()
    {
        if (currentLives <= 0) return;

        currentLives--;
        UpdateHearts();

        if (currentLives <= 0 && player != null)
        {
            player.Die();
            HandleGameOver();
        }
    }

    public void RepairOneLife()
    {
        if (currentLives >= maxLives) return;

        currentLives++;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartIcons.Count; i++)
        {
            if (heartIcons[i] != null)
            {
                heartIcons[i].SetActive(i < currentLives);
            }
        }
    }

    // Game Over

    void HandleGameOver()
    {
        isGameOver = true;
        scoreTimeAccumulator = 0f;

        if (bgScrolls != null)
        {
            foreach (var bg in bgScrolls)
            {
                if (bg != null) bg.enabled = false;
            }
        }

        if (enemySpawners != null)
        {
            foreach (var spawner in enemySpawners)
            {
                if (spawner != null) spawner.enabled = false;

            }

            if (player != null)
            {
                player.Die();
            }

            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }

            Debug.Log("GAME OVER");
        }
    }
}

