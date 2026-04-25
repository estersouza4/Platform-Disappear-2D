using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform player;

    public float velocidade = 2f;
    public float distanciaParaPerseguir = 8f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia <= distanciaParaPerseguir)
        {
            Vector2 direcao = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direcao * velocidade * Time.deltaTime);
        }
    }
}