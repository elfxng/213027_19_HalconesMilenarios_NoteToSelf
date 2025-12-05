using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoretext;

    // actual score
    private int scoreValue = 0;

    // Public property so that WinnerS sequence can read the score
    public int Score => scoreValue;

    public void AddPoint()
    {
        scoreValue++;
        scoretext.text = scoreValue.ToString();
    }
}
