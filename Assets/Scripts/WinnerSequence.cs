using UnityEngine;
using System.Collections;

public class WinnerSequence : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEntry
    {
        public string playerName;
        public Transform character;
        public ScoreManager score;
        public GameObject winsObject;
    }

    public PlayerEntry[] players;

    [Header("Timing")]
    public float delayAfterFinish = 1.5f;
    public float cameraMoveDuration = 2f;

    [Header("Camera")]
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, 0f);
    public Camera targetCamera;

    [Header("Camera Zoom")]
    public float finalZoom = 3.5f;

    [Header("UI")]
    public GameObject restartButton;   // 🔥 EL NUEVO BOTÓN

    private bool sequenceStarted = false;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Update()
    {
        if (!sequenceStarted && Timer.IsTimeUp)
        {
            sequenceStarted = true;
            StartCoroutine(WinnerRoutine());
        }
    }

    IEnumerator WinnerRoutine()
    {
        // 1) esperar un momento
        yield return new WaitForSeconds(delayAfterFinish);

        // 2) buscar el ganador
        PlayerEntry winner = null;
        int bestScore = int.MinValue;

        foreach (var p in players)
        {
            if (p == null || p.score == null) continue;

            int s = p.score.Score;

            if (winner == null || s > bestScore)
            {
                winner = p;
                bestScore = s;
            }
        }

        if (winner == null || targetCamera == null)
            yield break;

        // 3) preparar movimiento y zoom
        Vector3 startPos = targetCamera.transform.position;
        Vector3 targetPos = winner.character.position + cameraOffset;
        targetPos.z = startPos.z;

        float startOrthoSize = targetCamera.orthographicSize;
        float t = 0f;

        // 4) movimiento + zoom
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / cameraMoveDuration);

            targetCamera.transform.position = Vector3.Lerp(startPos, targetPos, f);
            targetCamera.orthographicSize = Mathf.Lerp(startOrthoSize, finalZoom, f);

            yield return null;
        }

        // 5) activar el WINS del ganador
        if (winner.winsObject != null)
            winner.winsObject.SetActive(true);

        // 6) 🔥 AHORA SÍ: activar el botón restart
        if (restartButton != null)
            restartButton.SetActive(true);
    }
}
