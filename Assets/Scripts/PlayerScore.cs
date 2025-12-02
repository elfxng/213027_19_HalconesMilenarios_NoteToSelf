using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [Header("Player info")]
    public string playerName = "Player";

    public int Score { get; private set; }

    public void AddPoint(int amount = 1)
    {
        Score += amount;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
