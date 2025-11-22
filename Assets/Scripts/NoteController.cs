using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NoteMover : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        SetRandomVelocity();
    }

    void FixedUpdate()
    {
        // 1) If time is over OR start is locked -> note stays still
        if (Timer.IsTimeUp || !GameStart.CanPlayersMove)
        {
            rb.linearVelocity = Vector2.zero;

            // When time is over, also clear stored velocity so it never moves again
            if (Timer.IsTimeUp)
            {
                velocity = Vector2.zero;
            }

            return;
        }

        // 2) Normal movement while game is running
        rb.linearVelocity = velocity;
    }

    // Bounce when it hits a wall
    void OnCollisionEnter2D(Collision2D col)
    {
        ContactPoint2D contact = col.contacts[0];
        Vector2 normal = contact.normal;

        // reflect velocity
        velocity = Vector2.Reflect(velocity, normal);

        // keep speed within range
        float speed = Random.Range(minSpeed, maxSpeed);
        velocity = velocity.normalized * speed;
    }

    void SetRandomVelocity()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float speed = Random.Range(minSpeed, maxSpeed);
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
    }
}
