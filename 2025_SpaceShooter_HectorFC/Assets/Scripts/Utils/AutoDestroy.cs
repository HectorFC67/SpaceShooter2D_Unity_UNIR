using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float lifeTime = 1.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
