using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float respawnTime = 4f;

    private Vector3 startPosition;
    private Vector3 originalScale;
    private bool isDead = false;

    private SpriteRenderer sr;
    private Collider2D col;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;

        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        OlharParaEsquerda();
    }
    void Update()
    {
        if (isDead) return;

        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    void OlharParaEsquerda()
    {
        transform.localScale = new Vector3(
            -Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
    }
    public void Defeat()
    {
        if (isDead) return;
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        isDead = true;

        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;
        if (rb != null) rb.simulated = false;

        yield return new WaitForSeconds(respawnTime);

        transform.position = startPosition;
        OlharParaEsquerda();

        if (sr != null) sr.enabled = true;
        if (col != null) col.enabled = true;
        if (rb != null) rb.simulated = true;

        isDead = false;
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