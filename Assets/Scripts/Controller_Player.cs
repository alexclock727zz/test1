using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Controller_Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 12f;
    public bool canDoubleJump = true;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private float moveX;
    private bool isGrounded;
    private int extraJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 3f;
    }

    void Update()
    {
        // Движение
        moveX = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        // Проверка земли
        if (groundCheckPoint != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        }
        else
        {
            // Если вы забыли создать groundCheckPoint, используем запасной метод через коллайдер
            isGrounded = col.IsTouchingLayers(groundLayer);
        }

        if (isGrounded) extraJumps = canDoubleJump ? 1 : 0;

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded) Jump();
            else if (extraJumps > 0)
            {
                Jump();
                extraJumps--;
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}