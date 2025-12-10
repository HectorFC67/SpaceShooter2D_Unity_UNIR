using UnityEngine;

public class BGScroll : MonoBehaviour
{
    [SerializeField] Transform[] layers;
    [SerializeField] float scrollSpeed = 2f;

    float tileSizeX;

    void Start()
    {
        SpriteRenderer sriteRenderer = layers[0].GetComponent<SpriteRenderer>();
        tileSizeX = sriteRenderer.bounds.size.x;
    }

    void Update()
    {
        foreach (Transform t in layers)
        {
            t.Translate(Vector2.left * scrollSpeed * Time.deltaTime);
        }

        float camLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;

        foreach (Transform t in layers)
        {
            float rightEdge = t.position.x + tileSizeX / 2f;
            if (rightEdge < camLeft)
            {
                Transform rightMost = layers[0];
                foreach (Transform other in layers)
                {
                    if (other.position.x > rightMost.position.x)
                        rightMost = other;
                }
                t.position = rightMost.position + Vector3.right * tileSizeX;
            }
        }
    }
}
