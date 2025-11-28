using UnityEngine;

public class NoteHit : MonoBehaviour
{
    public AudioClip hitSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            // 🔹 1. Tu sistema actual de puntos (NO SE TOCA)
            ScoreManager sm = collision.GetComponentInParent<ScoreManager>();
            if (sm != null)
                sm.AddPoint();

            // 🔹 2. NUEVO: sumar puntos al jugador que tocó la nota
            PlayerScore ps = collision.GetComponentInParent<PlayerScore>();
            if (ps != null)
                ps.AddPoint(1);

            // 🔹 3. Tu sistema de sonido (NO SE TOCA)
            AudioSource audio = collision.GetComponentInParent<AudioSource>();
            if (audio != null && hitSound != null)
                audio.PlayOneShot(hitSound);
        }
    }
}
