using UnityEngine;

public class NPCNoteCollector : MonoBehaviour
{
    [Tooltip("Assign the ScoreManager for this NPC (drag the GameManagerNPC object here)")]
    public ScoreManager myScore;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("MusicalNote"))
        {
            if (myScore != null)
                myScore.AddPoint();
            else
                Debug.LogWarning($"NPCNoteCollector: myScore not assigned on {gameObject.name}");
        }
    }
}
