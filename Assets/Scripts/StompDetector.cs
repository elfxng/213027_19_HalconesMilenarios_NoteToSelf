using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StompDetector : MonoBehaviour
{
    public float bounceForce = 8f;   // rebote hacia arriba después de pisar

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ¿El otro tiene Stompable?
        Stompable stompable = collision.collider.GetComponentInParent<Stompable>();
        if (stompable == null) return;

        // Revisar si el golpe fue desde ARRIBA
        foreach (var contact in collision.contacts)
        {
            // normal.y > 0.5f => ESTE objeto está encima del otro
            if (contact.normal.y > 0.5f)
            {
                // Aplicar efecto al que está debajo
                stompable.Stomp();

                // Rebote hacia arriba
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
                }

                break;
            }
        }
    }
}
