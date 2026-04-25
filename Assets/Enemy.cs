using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;

    private bool isDead = false;
    private Vector3 originalScale;

    private SpriteRenderer sr;
    private Collider2D col;
    private Rigidbody2D rb;

    void Start()
    {
        originalScale = transform.localScale;

        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (isDead) return;

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

   public void Defeat()
{
    if (isDead) return;

    isDead = true;

    GameManager.instance?.AddScore(10);

    Destroy(gameObject);
}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.Die();
            }
        }
    }
}