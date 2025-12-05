using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    [Min(0.01f)] public float groundCheckRadius = 0.18f;
    public LayerMask groundLayer = 0;

    [Header("Animation Tuning")]
    [Range(0f, 1f)] public float animIdleWalkThreshold = 0.5f;
    [Min(0f)] public float animBlendSpeed = 10f;

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

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _rb.freezeRotation = true;
    }

    void Update()
    {
        // Block movement while start lock OR when time is up
        if (Timer.IsTimeUp || !GameStart.CanPlayersMove)
        {
            _moveInput = 0f;
            _jumpQueued = false;

            _anim.SetFloat(HashSpeed, 0f);
            _anim.SetBool(HashGrounded, _isGrounded);
            _anim.SetFloat(HashYVel, _rb.linearVelocity.y);
            return;
        }

        // Face movement direction
        if (_sprite && Mathf.Abs(_moveInput) > 0.01f)
            _sprite.flipX = _moveInput < 0f;

        // Jump
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
        // Do not move horizontally while start lock or time is up
        if (Timer.IsTimeUp || !GameStart.CanPlayersMove)
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }
        else
        {
            _rb.linearVelocity = new Vector2(_moveInput * moveSpeed, _rb.linearVelocity.y);

            if (Mathf.Abs(_moveInput) < 0.01f)
                _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }

        if (groundCheck)
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }

    public void OnMove(InputValue value)
    {
        if (Timer.IsTimeUp || !GameStart.CanPlayersMove) return;
        _moveInput = Mathf.Clamp(value.Get<float>(), -1f, 1f);
    }

    public void OnJump(InputValue value)
    {
        // block jump if time is up or start is locked
        if (Timer.IsTimeUp || !GameStart.CanPlayersMove) return;

        // only allow jump if:
        // 1) button is pressed
        // 2) Mario is grounded
        // 3) he is not going up already (avoid double jump feeling)
        if (value.isPressed && _isGrounded && _rb.linearVelocity.y <= 0.01f)
        {
            _jumpQueued = true;
        }
    }

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
