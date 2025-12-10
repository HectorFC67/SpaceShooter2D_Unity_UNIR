using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpaceShip : MonoBehaviour
{
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float acceleration = 2f;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed = 4f;

    [SerializeField] int maxLives = 3;
    int currentLives;

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference shoot;

    private void OnEnable()
    {
        move.action.Enable();
        shoot.action.Enable();

        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        shoot.action.started += OnShoot;
    }

    void Awake()
    {
        currentLives = maxLives;
        Debug.Log("Vidas iniciales: " + currentLives);
    }

    Vector2 rawMove;
    Vector2 currentVelocity = Vector2.zero;
    const float rawMoveThresholdForBakings = 0.1f;
    void Update()
    {
        if (rawMove.magnitude < rawMoveThresholdForBakings)
        {
            currentVelocity *= 0.5f * Time.deltaTime;
        }
        currentVelocity += rawMove * acceleration * Time.deltaTime;

        float linearVelocity = currentVelocity.magnitude;
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);

        transform.Translate(currentVelocity * maxSpeed * Time.deltaTime);
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * bulletSpeed;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentLives--;
            Destroy(other.gameObject);

            Debug.Log("Vidas restantes: " + currentLives);

            if (currentLives <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Debug.Log("Game Over");
        gameObject.SetActive(false);
    }

}
