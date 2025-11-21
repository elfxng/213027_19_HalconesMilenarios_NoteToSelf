using UnityEngine;

public class NPCChaseNote : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 6f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GameObject note = FindClosestNote();
        if (note == null) return;

        Vector2 pos = transform.position;
        Vector2 npos = note.transform.position;

        float direction = Mathf.Sign(npos.x - pos.x);
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // salto si la nota est� por encima
        if (npos.y > pos.y + 0.3f && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
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
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
}
