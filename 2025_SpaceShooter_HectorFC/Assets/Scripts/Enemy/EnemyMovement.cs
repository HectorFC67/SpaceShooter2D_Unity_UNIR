using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] float speed = 3f;
    Vector3 linearVelocity = Vector3.left;

    [Header("Booster Drop")]
    [SerializeField] GameObject[] boosterPrefabs;
    [SerializeField, Range(0f, 1f)] float dropChance = 0.3f;

    [Header("VFX")]
    [SerializeField] GameObject explosionPrefab;

    void Update()
    {
        transform.Translate(linearVelocity * speed * Time.deltaTime);

        Camera cam = Camera.main;
        float xLeft = cam.ViewportToWorldPoint(new Vector3(-0.2f, 0f, 0f)).x;

        if (transform.position.x < xLeft) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Golpe al player");
        }
    }

    public void Kill(bool dropBooster = true)
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        SFXManager.Instance?.PlayEnemyExplode();
        if (dropBooster)
        {
            TrySpawnBooster();
        }

        GameManager.Instance?.OnEnemyKilled();
        Destroy(gameObject);
    }

    void TrySpawnBooster()
    {
        if (boosterPrefabs == null || boosterPrefabs.Length == 0)
            return;

        if (Random.value > dropChance)
            return;

        int index = Random.Range(0, boosterPrefabs.Length);
        GameObject boosterPrefab = boosterPrefabs[index];

        Instantiate(boosterPrefab, transform.position, Quaternion.identity);
    }
}
