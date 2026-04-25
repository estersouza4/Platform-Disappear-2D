using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    public bool estaSeMovendo { get; private set; }
    public bool estaNoGroundStart { get; private set; }
    [Header("Tiro")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    private Vector3 escalaOriginal;
    private bool olhandoParaDireita = true;

    [Header("Pulo Especial")]
    public float energiaMaxima = 100f;
    public float energiaAtual = 100f;
    public float custoPuloDuplo = 100f;
    public float regeneracaoEnergia = 15f;
    public bool podeUsarPuloDuplo = true;

    public Slider energySlider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        escalaOriginal = transform.localScale;
    
        if (energySlider != null)
        {
            energySlider.maxValue = energiaMaxima;
            energySlider.value = energiaAtual;
        }   
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        float currentSpeed = speed;

        estaSeMovendo = move != 0; 

        energiaAtual += regeneracaoEnergia * Time.deltaTime;

        if (energiaAtual > energiaMaxima)
        {
            energiaAtual = energiaMaxima;
        }
        if (energySlider != null)
        {   
            energySlider.value = energiaAtual;
        }

        if (isGrounded)
        {
            podeUsarPuloDuplo = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }

        rb.linearVelocity = new Vector2(move * currentSpeed, rb.linearVelocity.y);

        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (anim != null)
        {
            anim.SetBool("isWalking", move != 0);
        }

        if (transform.position.y < -12f)
        {
            Die();
        }

        if (move > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            olhandoParaDireita = true;
        }
        else if (move < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            olhandoParaDireita = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (anim != null)
            {
                anim.SetBool("isJumping", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            PuloDuploEspecial();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Atirar();
        }
    }
    void PuloDuploEspecial()
    {
        if (!isGrounded && podeUsarPuloDuplo && energiaAtual >= energiaMaxima)
        {
            energiaAtual = 0f;
            podeUsarPuloDuplo = false;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    void Atirar()
    {
        Debug.Log("Tentando atirar...");

        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab não foi colocado no Inspector.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("Fire Point não foi colocado no Inspector.");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Projétil criado!");

        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            if (olhandoParaDireita)
                projectileScript.SetDirection(1);
            else
                projectileScript.SetDirection(-1);
        }
        else
        {
            Debug.LogError("O prefab do projétil não tem o script Projectile.");
        }
    }

    private bool morreu = false;

    public void Die()
    {
        if (morreu) return;

        morreu = true;

        GameOverMenu gameOver = FindAnyObjectByType<GameOverMenu>();

        if (gameOver != null)
        {
            gameOver.MostrarGameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("GroundStart"))
    {
        if (anim != null)
        {
            anim.SetBool("isJumping", false);
        }
    }

    if (collision.gameObject.CompareTag("GroundStart"))
    {
        estaNoGroundStart = true;
    }

    if (collision.gameObject.CompareTag("Enemy"))
    {
        Die();
    }
  }
    void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("GroundStart"))
    {
        estaNoGroundStart = false;
    }
}

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}