using UnityEngine;

public class NPCChaseNote : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 3f;

    [Header("Salto")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.6f;          // tiempo mínimo entre saltos
    public float maxHorizontalJumpDist = 1.5f; // solo salta si la nota está cerca en X

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float lastJumpTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GameObject note = FindClosestNote();

        bool grounded = IsGrounded();

        // Si no hay notas, quieto (solo Idle)
        if (note == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            UpdateAnimations(0f, grounded);
            return;
        }

        Vector2 pos = transform.position;
        Vector2 npos = note.transform.position;

        // 1) Moverse en X hacia la nota
        float direction = Mathf.Sign(npos.x - pos.x);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // Voltear sprite según dirección
        if (direction != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = direction < 0;
        }

        // 2) Decidir si debe saltar
        float horizontalDist = Mathf.Abs(npos.x - pos.x);
        float verticalDist = npos.y - pos.y;

        bool canJumpByTime = Time.time - lastJumpTime > jumpCooldown;
        bool noteIsAbove = verticalDist > 0.4f;            // nota está más arriba
        bool noteCloseX = horizontalDist < maxHorizontalJumpDist; // nota no muy lejos en X

        bool shouldJump = grounded && canJumpByTime && noteIsAbove && noteCloseX;

        if (shouldJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
        }

        // 3) Actualizar animaciones
        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        UpdateAnimations(horizontalSpeed, grounded);
    }

    void UpdateAnimations(float horizontalSpeed, bool grounded)
    {
        if (animator == null) return;

        // Idle <-> Walk
        animator.SetFloat("Speed", horizontalSpeed);

        // Jump
        bool isJumping = !grounded;
        animator.SetBool("IsJumping", isJumping);
    }

    GameObject FindClosestNote()
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (var n in notes)
        {
            float d = (n.transform.position - myPos).sqrMagnitude;
            if (d < minDist)
            {
                minDist = d;
                closest = n;
            }
        }

        return closest;
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
