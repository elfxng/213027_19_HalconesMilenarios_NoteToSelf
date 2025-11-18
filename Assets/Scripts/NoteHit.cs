using UnityEngine;

public class NoteHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHead"))
        {
            ScoreManager.Instance.AddPoint();
        }
    }
}
