using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    public static bool CanPlayersMove { get; private set; }

    public float startLockDuration = 4f;

    IEnumerator Start()
    {
        CanPlayersMove = false;
        yield return new WaitForSeconds(startLockDuration);
        CanPlayersMove = true;
        Debug.Log("Juego iniciado, ahora CanPlayersMove = true");
    }
}
