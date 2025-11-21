using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class PlayerController2D : MonoBehaviour
{
    // ───────────────────────────── Inspector ─────────────────────────────
    [Header("Movement")]
    [Tooltip("Horizontal speed in units per second.")]
    public float moveSpeed = 6f;

    [Tooltip("Impulse applied when jumping.")]
    public float jumpForce = 12f;

    [Header("Ground Check")]
    [Tooltip("Point under the feet used to detect the ground.")]
    public Transform groundCheck;

    [Tooltip("Radius of the overlap circle used to detect the ground.")]
    [Min(0.01f)] public float groundCheckRadius = 0.18f;

    [Tooltip("Physics layer considered as ground.")]
    public LayerMask groundLayer = 0;

    [Header("Animation Tuning")]
    [Tooltip("Speed threshold to switch Idle <-> Walk (0..1).")]
    [Range(0f, 1f)] public float animIdleWalkThreshold = 0.5f;

    [Tooltip("How fast the animated speed blends toward the target value.")]
    [Min(0f)] public float animBlendSpeed = 10f;

    // ───────────────────────── Runtime (private) ─────────────────────────
    Rigidbody2D _rb;
    SpriteRenderer _sprite;
    Animator _anim;

    float _moveInput;
    bool _jumpQueued;
    bool _isGrounded;
    float _animSpeedSmoothed;

    static readonly int HashSpeed = Animator.StringToHash("Speed");
    static readonly int HashGrounded = Animator.StringToHash("Grounded");
    static readonly int HashYVel = Animator.StringToHash("YVel");
    static readonly int HashJumpTrig = Animator.StringToHash("JumpTrig");

    // ───────────────────────────── Unity Flow ────────────────────────────
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponent<Animator>();

        _rb.freezeRotation = true;
    }

    void Update()
    {
        // 🟡 DETENER a Mario cuando el tiempo se acabe
        if (Timer.IsTimeUp)
        {
            _moveInput = 0f;
            _jumpQueued = false;

            // mantener animación Idle
            _anim.SetFloat(HashSpeed, 0f);
            _anim.SetBool(HashGrounded, _isGrounded);
            return;
        }

        // ───────── lógica original intacta ─────────
        if (_sprite && Mathf.Abs(_moveInput) > 0.01f)
            _sprite.flipX = _moveInput < 0f;

        if (_jumpQueued && _isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _jumpQueued = false;
            _anim.SetTrigger(HashJumpTrig);
        }

        float targetAnimSpeed = Mathf.Abs(_moveInput) >= animIdleWalkThreshold ? 1f : 0f;
        _animSpeedSmoothed = Mathf.MoveTowards(_animSpeedSmoothed, targetAnimSpeed, animBlendSpeed * Time.deltaTime);

        _anim.SetFloat(HashSpeed, _animSpeedSmoothed);
        _anim.SetBool(HashGrounded, _isGrounded);
        _anim.SetFloat(HashYVel, _rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // 🟡 SI SE ACABÓ EL TIEMPO → bloquear movimiento horizontal
        if (Timer.IsTimeUp)
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
            return;
        }

        // ───────── lógica original intacta ─────────
        _rb.linearVelocity = new Vector2(_moveInput * moveSpeed, _rb.linearVelocity.y);

        if (Mathf.Abs(_moveInput) < 0.01f)
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);

        if (groundCheck)
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    // ───────────────────────────── Input System ──────────────────────────
    public void OnMove(InputValue value)
    {
        if (Timer.IsTimeUp) return; // bloquear input
        _moveInput = Mathf.Clamp(value.Get<float>(), -1f, 1f);
    }

    public void OnJump(InputValue value)
    {
        if (Timer.IsTimeUp) return; // bloquear salto
        if (value.isPressed) _jumpQueued = true;
    }

    // ───────────────────────────── Utilities ─────────────────────────────
    void OnDisable()
    {
        _moveInput = 0f;
        _jumpQueued = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
