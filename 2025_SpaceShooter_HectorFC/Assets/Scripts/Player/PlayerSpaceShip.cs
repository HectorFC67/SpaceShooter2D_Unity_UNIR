using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerPowerUps))]
public class PlayerSpaceShip : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float acceleration = 2f;
    public float MaxSpeed           // propiedad para que los boosters puedan modificar la velocidad
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed = 4f;

    [SerializeField] float baseFireCooldown = 0.4f;
    [SerializeField] float machineGunFireCooldown = 0.1f;

    [Header("Lives")]
    [SerializeField] int maxLives = 3;
    public int MaxLives => maxLives;

    [Header("Input")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference shoot;

    [SerializeField] GameObject explosionPrefab;

    // Internos movimiento
    Vector2 rawMove;
    Vector2 currentVelocity = Vector2.zero;
    const float rawMoveThresholdForBakings = 0.1f;

    // Disparo
    float shootCooldownTimer = 0f;

    // Referencia a boosters
    PlayerPowerUps powerUps;

    private void OnEnable()
    {
        move.action.Enable();
        shoot.action.Enable();

        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        shoot.action.started += OnShoot;
    }

    private void Awake()
    {
        powerUps = GetComponent<PlayerPowerUps>();
        powerUps.Initialize(this);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // cooldown disparo
        shootCooldownTimer -= dt;

        // Movimiento
        if (rawMove.magnitude < rawMoveThresholdForBakings)
        {
            currentVelocity *= 0.5f * dt;
        }

        currentVelocity += rawMove * acceleration * dt;
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);

        transform.Translate(currentVelocity * dt);
    }

    private void OnDisable()
    {
        move.action.Disable();
        shoot.action.Disable();

        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        shoot.action.started -= OnShoot;
        shoot.action.performed -= OnShoot;
        shoot.action.canceled -= OnShoot;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        rawMove = obj.ReadValue<Vector2>();
    }

    private void OnShoot(InputAction.CallbackContext obj)
    {
        if (powerUps == null) return;

        float currentCooldown = powerUps.IsMachineGunActive
            ? machineGunFireCooldown
            : baseFireCooldown;

        if (shootCooldownTimer > 0f)
            return;

        shootCooldownTimer = currentCooldown;

        SFXManager.Instance?.PlayAllyShoot();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * bulletSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Colisión con enemigo
        if (other.CompareTag("Enemy"))
        {
            if (powerUps != null && powerUps.IsInvincible)
            {
                SFXManager.Instance?.PlayShieldHit();
                Destroy(other.gameObject);
                Debug.Log("Golpe bloqueado por escudo (invencible)");
                return;
            }

            Destroy(other.gameObject);
            SFXManager.Instance?.PlayHit();
            GameManager.Instance?.OnPlayerHit();
            return;
        }

        // Boosters por tag
        if (powerUps == null) return;

        if (other.CompareTag("BoosterInvencibilidad"))
        {
            SFXManager.Instance?.PlayPowerUpPickup();
            powerUps.ActivateInvincibility();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BoosterMetralleta"))
        {
            SFXManager.Instance?.PlayPowerUpPickup();
            powerUps.ActivateMachineGun();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BoosterReparacion"))
        {
            SFXManager.Instance?.PlayPowerUpPickup();
            powerUps.RepairOneLife();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BoosterVelocidad"))
        {
            SFXManager.Instance?.PlayPowerUpPickup();
            powerUps.ActivateSpeedBoost();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("BoosterNuclear"))
        {
            SFXManager.Instance?.PlayPowerUpPickup();
            powerUps.ActivateNuclearBomb();
            Destroy(other.gameObject);
        }
    }

    public void Die()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Debug.Log("Game Over - Player desactivado");
        gameObject.SetActive(false);
    }
}
