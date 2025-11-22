using UnityEngine;

public class NoteHit : MonoBehaviour
{
    public AudioClip hitSound; // sonido del golpe

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            
            ScoreManager sm = collision.GetComponentInParent<ScoreManager>();
            if (sm != null)
                sm.AddPoint();

            
            AudioSource audio = collision.GetComponentInParent<AudioSource>();
            if (audio != null && hitSound != null)
                audio.PlayOneShot(hitSound);
        }
    }
}
