using UnityEngine;

public class BoosterMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        Camera cam = Camera.main;
        float xLeft = cam.ViewportToWorldPoint(new Vector3(-0.2f, 0f, 0f)).x;

        if (transform.position.x < xLeft)
        {
            Destroy(gameObject);
        }
    }
}
