using System.Collections;
using UnityEngine;
using TMPro;   // si usas TextMeshPro

public class WinnerCamera : MonoBehaviour
{
    [Header("Camera")]
    public Camera targetCamera;          // si lo dejas vacío, usará Camera.main
    public float delayAfterFinish = 1.2f; // tiempo de espera después de FINISH
    public float moveDuration = 1.5f;    // tiempo que dura el movimiento/zoom
    public float zoomSize = 3f;          // tamaño ortográfico final (más pequeño = más zoom)

    [Header("Players")]
    public PlayerScore[] players;        // arrastra aquí a Mario, Luigi, Princesa, etc.

    [Header("Win UI")]
    public GameObject winPanel;          // panel o imagen con "WIN"
    public TMP_Text winText;             // texto opcional: "Mario WIN!"

    private bool sequenceStarted = false;
    private Vector3 originalCamPos;
    private float originalCamSize;

    void Start()
    {
        // Cámara
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera != null)
        {
            originalCamPos = targetCamera.transform.position;

            if (targetCamera.orthographic)
                originalCamSize = targetCamera.orthographicSize;
        }

        // Panel WIN oculto al inicio
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    void Update()
    {
        // Esperar a que el tiempo se acabe (Timer.IsTimeUp debe existir en tu Timer)
        if (!sequenceStarted && Timer.IsTimeUp)
        {
            sequenceStarted = true;
            StartCoroutine(FocusWinnerRoutine());
        }
    }

    IEnumerator FocusWinnerRoutine()
    {
        // 1) Esperar un poquito para que se vea bien el FINISH
        yield return new WaitForSeconds(delayAfterFinish);

        // 2) Buscar el ganador
        PlayerScore winner = GetWinner();
        if (winner == null || targetCamera == null)
        {
            if (winPanel != null) winPanel.SetActive(true);
            if (winText != null) winText.text = "WIN!";
            yield break;
        }

        // 3) Animar movimiento y zoom de la cámara hacia el ganador
        Vector3 startPos = targetCamera.transform.position;
        Vector3 targetPos = new Vector3(
            winner.transform.position.x,
            winner.transform.position.y,
            startPos.z
        );

        float startSize = targetCamera.orthographic ? targetCamera.orthographicSize : 0f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            float k = Mathf.SmoothStep(0f, 1f, t);

            targetCamera.transform.position = Vector3.Lerp(startPos, targetPos, k);

            if (targetCamera.orthographic)
                targetCamera.orthographicSize = Mathf.Lerp(startSize, zoomSize, k);

            yield return null;
        }

        // 4) Mostrar panel WIN con animación de "pop"
        if (winPanel != null)
        {
            winPanel.SetActive(true);

            Transform panelTransform = winPanel.transform;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one;

            panelTransform.localScale = startScale;

            float t2 = 0f;
            float animDuration = 0.5f;

            while (t2 < 1f)
            {
                t2 += Time.deltaTime / animDuration;
                float k2 = Mathf.SmoothStep(0f, 1f, t2);
                panelTransform.localScale = Vector3.Lerp(startScale, endScale, k2);
                yield return null;
            }
        }

        // Texto de ganador (opcional)
        if (winText != null)
        {
            winText.text = $"{winner.playerName} WIN!";
        }
    }

    PlayerScore GetWinner()
    {
        if (players == null || players.Length == 0) return null;

        PlayerScore best = null;
        int bestScore = int.MinValue;

        foreach (var p in players)
        {
            if (p == null) continue;
            if (p.Score > bestScore)
            {
                bestScore = p.Score;
                best = p;
            }
        }

        return best;
    }
}
