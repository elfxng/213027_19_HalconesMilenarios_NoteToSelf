using UnityEngine;

public class NPCNoteCollector : MonoBehaviour
{
    public ScoreManager myScore;

    private void Awake()
    {
        if (myScore == null)
            myScore = GetComponentInParent<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("MusicalNote"))
        {
            if (myScore != null)
                myScore.AddPoint();
            else
                Debug.LogWarning($"NPCNoteCollector: myScore not found for {gameObject.name}");
        }
    }
}
