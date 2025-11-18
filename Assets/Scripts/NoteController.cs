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
        rb.gravityScale = 0;           // No gravedad
        rb.freezeRotation = true;      // No girar
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        SetRandomVelocity();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = velocity;
    }

    // Rebotar cuando choca contra un muro
    void OnCollisionEnter2D(Collision2D col)
    {
        ContactPoint2D contact = col.contacts[0];
        Vector2 normal = contact.normal;

        // reflejar velocidad
        velocity = Vector2.Reflect(velocity, normal);

        // mantener velocidad constante
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
