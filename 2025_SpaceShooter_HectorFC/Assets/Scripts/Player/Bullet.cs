using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyMovement enemy = other.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                enemy.Kill();
            }
            else
            {
                Destroy(other.gameObject); // Destroy por si no tiene el script EnemyMovement (seguridad)
            }

            Destroy(gameObject);
        }
    }
}
