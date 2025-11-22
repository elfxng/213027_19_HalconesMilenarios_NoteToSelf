using System.Collections;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public static GameStart Instance { get; private set; }

    // If there is no GameStart in the scene, movement will be allowed by default
    public static bool CanPlayersMove
    {
        get
        {
            if (Instance == null) return true;
            return Instance._canMove;
        }
    }

    [Header("Start lock")]
    public float startLockDuration = 4f;   // seconds players are blocked at the beginning

    [Header("Optional start animation")]
    public Animator startAnimator;         // UI or object animator (Ready/Go)
    public string startTriggerName = "Start"; // trigger name in that animator

    bool _canMove = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    IEnumerator Start()
    {
        // Play start animation if you have one
        if (startAnimator != null && !string.IsNullOrEmpty(startTriggerName))
        {
            startAnimator.SetTrigger(startTriggerName);
        }

        // Lock movement for a few seconds
        _canMove = false;
        yield return new WaitForSeconds(startLockDuration);

        // Now players can move
        _canMove = true;
    }
}
