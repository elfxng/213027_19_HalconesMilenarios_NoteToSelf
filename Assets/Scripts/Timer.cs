using UnityEngine;
using TMPro; // elimina si usas Text normal
using UnityEngine.UI; // deja si usas Text

public class Timer : MonoBehaviour
{
    public float timeLeft = 60f;      // duración del cronómetro (cambiado a 60)
    public TMP_Text timerText;        // cambia a Text si no usas TMP
    private bool running = true;

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            timeLeft = 0;
            running = false;
        }

        timerText.text = Mathf.Ceil(timeLeft).ToString();
    }
}
