using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        float currentSpeed = speed;

        estaSeMovendo = move != 0; 

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

        if (Input.GetKeyDown(KeyCode.X))
        {
            Atirar();
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

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (anim != null)
            {
                anim.SetBool("isJumping", false);
            }
        }
        if (collision.gameObject.CompareTag("GroundStart")) // NOVO
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