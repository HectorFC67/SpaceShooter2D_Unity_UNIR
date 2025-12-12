using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    [Header("Boosters - Settings")]
    [SerializeField] GameObject shieldVisual;
    [SerializeField] float invincibilityDuration = 5f;
    [SerializeField] float machineGunDuration = 7f;
    [SerializeField] float speedBoostDuration = 6f;
    [SerializeField] float speedBoostMultiplier = 1.5f;

    // Timers
    float invincibilityTimer = 0f;
    float machineGunTimer = 0f;
    float speedBoostTimer = 0f;

    // Referencia al player para tocar velocidad
    PlayerSpaceShip player;
    float baseMaxSpeed;

    public bool IsInvincible => invincibilityTimer > 0f;
    public bool IsMachineGunActive => machineGunTimer > 0f;

    public void Initialize(PlayerSpaceShip playerSpaceShip)
    {
        player = playerSpaceShip;
        baseMaxSpeed = player.MaxSpeed;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= dt;
            if (invincibilityTimer <= 0f && shieldVisual != null)
            {
                shieldVisual.SetActive(false);
            }
        }

        if (machineGunTimer > 0f)
        {
            machineGunTimer -= dt;
        }

        if (speedBoostTimer > 0f)
        {
            speedBoostTimer -= dt;
            if (speedBoostTimer <= 0f && player != null)
            {
                player.MaxSpeed = baseMaxSpeed;
            }
        }
    }

    public void ActivateInvincibility()
    {
        invincibilityTimer = invincibilityDuration;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        Debug.Log($"Booster invencibilidad activado durante {invincibilityDuration}s");
    }

    public void ActivateMachineGun()
    {
        machineGunTimer = machineGunDuration;
        Debug.Log($"Booster metralleta activado durante {machineGunDuration}s");
    }

    public void RepairOneLife()
    {
        GameManager.Instance?.RepairOneLife();
    }

    public void ActivateSpeedBoost()
    {
        if (player == null) return;

        speedBoostTimer = speedBoostDuration;
        player.MaxSpeed = baseMaxSpeed * speedBoostMultiplier;

        Debug.Log($"Booster velocidad activado: maxSpeed = {player.MaxSpeed} durante {speedBoostDuration}s");
    }

    public void ActivateNuclearBomb()
    {
        Debug.Log("Booster nuclear activado: destruyendo todos los enemigos");

        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Kill(false);
            }
        }
    }
}
