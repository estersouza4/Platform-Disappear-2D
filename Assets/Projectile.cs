using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 2f;

    private int direction = 1;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
    public void SetDirection(int newDirection)
    {
        direction = newDirection;

        if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.Defeat();
            }

            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}