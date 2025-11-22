using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }   // para acceder desde otros scripts
    public static bool IsTimeUp => Instance != null && Instance.timeLeft <= 0f;

    [Header("Tiempo")]
    public float timeLeft = 30f;      // duración del cronómetro

    [Header("Texto UI")]
    public TMP_Text timerText;        // si usas TextMeshPro

    private bool running = true;

    void Awake()
    {
        // patrón Singleton muy simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            running = false;
        }

        if (timerText != null)
            timerText.text = Mathf.Ceil(timeLeft).ToString();
    }
}
