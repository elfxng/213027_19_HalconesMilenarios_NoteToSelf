using UnityEngine;

public class NoteHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Golpeó una cabeza
        if (collision.CompareTag("PlayerHead"))
        {
            // Buscar el ScoreManager del personaje dueño de esa cabeza
            ScoreManager sm = collision.GetComponentInParent<ScoreManager>();

            if (sm != null)
                sm.AddPoint();
        }
    }
}
