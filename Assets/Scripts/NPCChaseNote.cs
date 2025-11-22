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

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float lastJumpTime = -999f;
    private bool hasJumped = false;   // avoid double jump
    private bool wasGrounded = false; // track landing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        bool grounded = IsGrounded();

        // 1) If time is up OR start is locked -> do not move, just fall
        if (Timer.IsTimeUp || !GameStart.CanPlayersMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            float endSpeed = Mathf.Abs(rb.linearVelocity.x);
            UpdateAnimations(endSpeed, grounded);
            wasGrounded = grounded;
            return;
        }

        // 2) Detect landing: only when we were in the air and now we are grounded
        if (grounded && !wasGrounded)
        {
            hasJumped = false; // allow a new jump after landing
        }

        GameObject note = FindClosestNote();

        // 3) No notes -> idle
        if (note == null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            UpdateAnimations(0f, grounded);
            wasGrounded = grounded;
            return;
        }

        Vector2 pos = transform.position;
        Vector2 npos = note.transform.position;

        // 4) Move horizontally toward the note
        float direction = Mathf.Sign(npos.x - pos.x);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // Flip sprite
        if (direction != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = (direction < 0);
        }

        // 5) Decide if the NPC should jump
        float horizontalDist = Mathf.Abs(npos.x - pos.x);
        float verticalDist = npos.y - pos.y;

        bool canJumpByTime = Time.time - lastJumpTime > jumpCooldown;
        bool noteAbove = verticalDist > 0.4f;
        bool closeEnoughX = horizontalDist < maxHorizontalJumpDist;

        // 👉 Only one jump until landing: !hasJumped && grounded
        bool shouldJump = !hasJumped && grounded && canJumpByTime && noteAbove && closeEnoughX;

        if (shouldJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpTime = Time.time;
            hasJumped = true;
        }

        // 6) Update animations
        float horizSpeed = Mathf.Abs(rb.linearVelocity.x);
        UpdateAnimations(horizSpeed, grounded);

        // Save grounded state for next frame
        wasGrounded = grounded;
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
        // IMPORTANT: groundLayer should contain ONLY the floor/platforms
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
