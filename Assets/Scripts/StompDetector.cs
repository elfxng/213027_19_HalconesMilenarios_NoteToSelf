using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StompDetector : MonoBehaviour
{
    public float bounceForce = 8f;   // rebound upwards after stepping

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        Stompable stompable = collision.collider.GetComponentInParent<Stompable>();
        if (stompable == null) return;

        // Check if the impact came from ABOVE
        foreach (var contact in collision.contacts)
        {
            
            if (contact.normal.y > 0.5f)
            {
                // Apply effect to the one below
                stompable.Stomp();

                // Bounce upwards
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                }

                break;
            }
        }
    }
}
