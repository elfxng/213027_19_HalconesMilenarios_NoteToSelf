using UnityEngine;

public class NPCNoteCollector : MonoBehaviour
{
    private float lastPointTime = 0f;
    public float cooldown = 0.3f; // evitar sumar mil puntos por segundo

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Note"))
        {
            if (Time.time - lastPointTime > cooldown)
            {
                ScoreManager.Instance.AddPoint();
                lastPointTime = Time.time;
            }
        }
    }
}
