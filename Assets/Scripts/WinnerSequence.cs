using UnityEngine;
using System.Collections;

public class WinnerSequence : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEntry
    {
        public string playerName;      // solo para identificarlo
        public Transform character;    // objeto del jugador en escena
        public ScoreManager score;     // script de puntos de ese jugador
        public GameObject winsObject;  // hijo con el sprite "Wins"
    }

    public PlayerEntry[] players;

    [Header("Timing")]
    public float delayAfterFinish = 1.5f;   // esperar después de que termine el tiempo
    public float cameraMoveDuration = 2f;   // tiempo del movimiento/zoom de cámara

    [Header("Camera")]
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, 0f);
    public Camera targetCamera;

    [Header("Camera Zoom")]
    public float finalZoom = 3.5f;          // más pequeño = más zoom

    private bool sequenceStarted = false;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Update()
    {
        // solo arrancar una vez cuando el tiempo se acabe
        if (!sequenceStarted && Timer.IsTimeUp)
        {
            sequenceStarted = true;
            StartCoroutine(WinnerRoutine());
        }
    }

    IEnumerator WinnerRoutine()
    {
        // 1) esperar un momento (en este tiempo ya se mostró FINISH)
        yield return new WaitForSeconds(delayAfterFinish);

        // 2) buscar el jugador con más puntos
        PlayerEntry winner = null;
        int bestScore = int.MinValue;

        foreach (var p in players)
        {
            if (p == null || p.score == null) continue;

            int s = p.score.Score;   // propiedad pública del ScoreManager

            if (winner == null || s > bestScore)
            {
                winner = p;
                bestScore = s;
            }
        }

        if (winner == null || targetCamera == null)
            yield break;

        // 3) preparar movimiento + zoom de cámara
        Vector3 startPos = targetCamera.transform.position;
        Vector3 targetPos = winner.character.position + cameraOffset;
        targetPos.z = startPos.z; // mantener la misma Z

        float startOrthoSize = targetCamera.orthographicSize;
        float t = 0f;

        // 4) mover cámara y hacer zoom
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / cameraMoveDuration);

            // mover posición
            targetCamera.transform.position = Vector3.Lerp(startPos, targetPos, f);

            // zoom (para cámara ortográfica 2D)
            targetCamera.orthographicSize = Mathf.Lerp(startOrthoSize, finalZoom, f);

            yield return null;
        }

        // 5) activar el cartel WINS del ganador
        if (winner.winsObject != null)
            winner.winsObject.SetActive(true);
    }
}
