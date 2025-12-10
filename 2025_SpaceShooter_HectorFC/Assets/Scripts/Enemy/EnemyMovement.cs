using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    Vector3 linearVelocity = Vector3.left;

    void Update()
    {
        transform.Translate(linearVelocity * speed * Time.deltaTime);

        Camera cam = Camera.main;
        float xLeft = cam.ViewportToWorldPoint(new Vector3(-0.2f, 0f, 0f)).x;

        if (transform.position.x < xLeft)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Golpe al player");
        }
    }
}