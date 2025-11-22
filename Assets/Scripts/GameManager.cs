using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool gameStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        gameStarted = true;
    }
}
