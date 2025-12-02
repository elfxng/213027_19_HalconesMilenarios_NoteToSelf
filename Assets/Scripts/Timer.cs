using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Game Time")]
    public float gameDuration = 30f;   // segundos reales de juego
    public TMP_Text timerText;         // texto que muestra el tiempo

    public static bool IsTimeUp = false;

    private float elapsedTime = 0f;    // cuánto tiempo de juego ha pasado
    private bool finished = false;

    void Awake()
    {
        IsTimeUp = false;
        elapsedTime = 0f;
        finished = false;
    }

    void Update()
    {
        // si ya terminó, no hacemos nada
        if (finished) return;

        // mientras el juego NO haya empezado, el tiempo NO avanza
        if (!GameStart.CanPlayersMove)
        {
            if (timerText != null)
                timerText.text = Mathf.Ceil(gameDuration).ToString();
            return;
        }

        // A partir de aquí el juego YA empezó
        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Max(0f, gameDuration - elapsedTime);

        if (timerText != null)
            timerText.text = Mathf.Ceil(remaining).ToString();

        if (remaining <= 0f)
        {
            finished = true;
            IsTimeUp = true;
            Debug.Log("Timer terminó, 30 segundos de juego completados");
        }
    }
}
