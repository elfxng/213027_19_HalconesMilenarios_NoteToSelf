using UnityEngine;

public class NPCChaseNote : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.6f;
    public float maxHorizontalJumpDist = 1.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    float lastJumpTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool grounded = IsGrounded();

        // STOP MOVEMENT ONLY WHEN TIME IS OVER
        if (Timer.IsTimeUp)
        {
            // no horizontal movement, let gravity work
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            float endSpeed = Mathf.Abs(rb.linearVelocity.x);
            UpdateAnimations(endSpeed, grounded);
            return;
        }

        // ---------- NORMAL AI WHILE THERE IS TIME ----------
        GameObject note = FindClosestNote();

        // No notes = idle
        if (note == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            UpdateAnimations(0f, grounded);
            return;
        }

        Vector2 pos = transform.position;
        Vector2 npos = note.transform.position;

        // Horizontal movement
        float direction = Mathf.Sign(npos.x - pos.x);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // Flip sprite
        if (direction != 0 && spriteRenderer != null)
            spriteRenderer.flipX = (direction < 0);

        // Jump logic
        float horizontalDist = Mathf.Abs(npos.x - pos.x);
        float verticalDist = npos.y - pos.y;

        bool canJumpByTime = Time.time - lastJumpTime > jumpCooldown;
        bool noteAbove = verticalDist > 0.4f;
        bool closeEnoughX = horizontalDist < maxHorizontalJumpDist;

        bool shouldJump = grounded && canJumpByTime && noteAbove && closeEnoughX;

        if (shouldJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
        }

        float horizSpeed = Mathf.Abs(rb.linearVelocity.x);
        UpdateAnimations(horizSpeed, grounded);
    }

    void UpdateAnimations(float horizontalSpeed, bool grounded)
    {
        if (animator == null) return;

        animator.SetFloat("Speed", horizontalSpeed);
        animator.SetBool("IsJumping", !grounded);
    }

    GameObject FindClosestNote()
    {
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");
        if (notes.Length == 0) return null;

        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (var n in notes)
        {
            float dist = (n.transform.position - myPos).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
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
