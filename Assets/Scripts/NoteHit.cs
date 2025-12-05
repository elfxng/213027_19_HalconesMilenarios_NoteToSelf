using UnityEngine;

public class NoteHit : MonoBehaviour
{
    public AudioClip hitSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            // Your current points system
            ScoreManager sm = collision.GetComponentInParent<ScoreManager>();
            if (sm != null)
                sm.AddPoint();

            //  Add points to the player who hit the note
            PlayerScore ps = collision.GetComponentInParent<PlayerScore>();
            if (ps != null)
                ps.AddPoint(1);

            // Your sound system 
            AudioSource audio = collision.GetComponentInParent<AudioSource>();
            if (audio != null && hitSound != null)
                audio.PlayOneShot(hitSound);
        }
    }
}
