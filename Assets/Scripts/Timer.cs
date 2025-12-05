using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Game Time")]
    public float gameDuration = 30f;   // actual seconds of gameplay
    public TMP_Text timerText;         // Text that displays the time

    public static bool IsTimeUp = false;

    [Header("UI End")]
    public GameObject restartButton;      // reset button
    public CountdownUI countdownUI;       // reference to the script that displays FINISH

    private float elapsedTime = 0f;    
    private bool finished = false;

    void Awake()
    {
        IsTimeUp = false;
        elapsedTime = 0f;
        finished = false;

        if (restartButton != null)
            restartButton.SetActive(false);   
    }

    void Update()
    {
        
        if (finished) return;

        // While the game has NOT started, time DOES NOT advance
        if (!GameStart.CanPlayersMove)
        {
            if (timerText != null)
                timerText.text = Mathf.Ceil(gameDuration).ToString();
            return;
        }

        // From here the game has already begun
        elapsedTime += Time.deltaTime;

        float remaining = Mathf.Max(0f, gameDuration - elapsedTime);

        if (timerText != null)
            timerText.text = Mathf.Ceil(remaining).ToString();

        if (remaining <= 0f)
        {
            finished = true;
            IsTimeUp = true;
            Debug.Log("Timer terminó, 30 segundos de juego completados");

            
            if (countdownUI != null)
                countdownUI.ShowFinish();

            
            if (restartButton != null)
                restartButton.SetActive(true);
        }
    }
}
