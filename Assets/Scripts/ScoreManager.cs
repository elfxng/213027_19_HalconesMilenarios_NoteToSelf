using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoretext;

    // Puntaje real
    private int scoreValue = 0;

    // 🔥 Propiedad pública para que WinnerSequence pueda leer el puntaje
    public int Score => scoreValue;

    public void AddPoint()
    {
        scoreValue++;
        scoretext.text = scoreValue.ToString();
    }
}
