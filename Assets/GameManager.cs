using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public float timeToAddPoint = 1f;
    public TextMeshProUGUI scoreText;

    private float timer = 0f;
    private PlayerMovement player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        AtualizarTextoPontuacao();
    }

    void Update()
    {
        if (player == null)
            return;

        bool podeContarPonto = !player.estaNoGroundStart && player.estaSeMovendo;

        if (podeContarPonto)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAddPoint)
            {
                score += 1;
                timer = 0f;
                AtualizarTextoPontuacao();
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        AtualizarTextoPontuacao();
    }

    void AtualizarTextoPontuacao()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("D3");
        }
    }
}