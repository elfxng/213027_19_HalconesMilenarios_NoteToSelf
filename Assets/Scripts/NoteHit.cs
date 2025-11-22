using UnityEngine;

public class NoteHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("PlayerHead"))
        {
            
            ScoreManager sm = collision.GetComponentInParent<ScoreManager>();

            if (sm != null)
                sm.AddPoint();
        }
    }
}
