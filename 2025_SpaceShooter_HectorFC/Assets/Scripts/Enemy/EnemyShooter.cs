using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed = 5f;

    [SerializeField] float minShootTime = 1.5f;
    [SerializeField] float maxShootTime = 3.5f;

    float shootTimer;

    void Start()
    {
        ResetShootTimer();
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            ResetShootTimer();
        }
    }

    void ResetShootTimer()
    {
        shootTimer = Random.Range(minShootTime, maxShootTime);
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.left * bulletSpeed;
        }
    }
}
