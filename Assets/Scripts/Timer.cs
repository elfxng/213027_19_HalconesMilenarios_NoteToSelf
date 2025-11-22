using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }   
    public static bool IsTimeUp => Instance != null && Instance.timeLeft <= 0f;

    [Header("Tiempo")]
    public float timeLeft = 30f;      // stopwatch duration

    [Header("Texto UI")]
    public TMP_Text timerText;        

    private bool running = true;

    void Awake()
    {
        
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
