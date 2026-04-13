using System.Collections;
using UnityEngine;
public class DisappearingPlatform : MonoBehaviour
{
    public float tempoParaSumir = 1f;
    public float tempoParaVoltar = 2f;

    public int quantidadePiscadas = 3;
    public float intervaloPiscada = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private bool estaSumindo = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !estaSumindo)
        {
            StartCoroutine(SumirEVoltar());
        }
    }
    IEnumerator SumirEVoltar()
    {
        estaSumindo = true;

        yield return new WaitForSeconds(tempoParaSumir);

        // pisca antes de sumir
        for (int i = 0; i < quantidadePiscadas; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(intervaloPiscada);

            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(intervaloPiscada);
        }
        // some de verdade
        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        yield return new WaitForSeconds(tempoParaVoltar);

        // volta
        spriteRenderer.enabled = true;
        platformCollider.enabled = true;

        estaSumindo = false;
    }
}