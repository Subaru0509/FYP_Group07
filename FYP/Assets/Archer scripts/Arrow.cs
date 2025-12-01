using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private int direction;

    public void SetDirection(int dir)
    {
        direction = dir;
        transform.localScale = new Vector3(dir, 1, 1); // Reverse the direction of the arrow
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Entity_Health health = collision.GetComponent<Entity_Health>();
            if (health != null)
            {
                health.TakeDamage(damage, transform);
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
}

