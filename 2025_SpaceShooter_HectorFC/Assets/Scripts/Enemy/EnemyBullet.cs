using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 4f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
