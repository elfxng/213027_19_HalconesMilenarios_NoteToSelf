using UnityEngine;
using System.Collections;

public class WinnerSequence : MonoBehaviour
{
    [System.Serializable]
    public class PlayerEntry
    {
        public string playerName;      // solo para verlo en el inspector
        public Transform character;    // objeto del jugador en la escena
        public ScoreManager score;     // script de puntaje de ese jugador
        public GameObject winsObject;  // hijo con el sprite "Wins"
    }

    public PlayerEntry[] players;

    [Header("Timing")]
    public float delayAfterFinish = 1.5f;   // esperar después de que aparezca FINISH
    public float cameraMoveDuration = 2f;   // tiempo que tarda la cámara en acercarse

    [Header("Camera")]
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, 0f);
    public Camera targetCamera;

    [Header("Finish Panel")]
    public GameObject finishPanel;          // objeto FINISH del Canvas

    private bool sequenceStarted = false;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Update()
    {
        // si ya empezó, no repetimos
        if (sequenceStarted) return;

        // cuando el tiempo llega a 0
        if (Timer.IsTimeUp)
        {
            sequenceStarted = true;
            StartCoroutine(WinnerRoutine());
        }
    }

    IEnumerator WinnerRoutine()
    {
        // 1) dejamos que FINISH se vea un momento
        yield return new WaitForSeconds(delayAfterFinish);

        // 🔥 ahora sí ocultamos FINISH
        if (finishPanel != null)
            finishPanel.SetActive(false);

        // 2) buscamos el jugador con más puntos
        PlayerEntry winner = null;
        int bestScore = int.MinValue;

        foreach (var p in players)
        {
            if (p == null || p.score == null) continue;

            int s = p.score.Score;   // usamos la propiedad pública del ScoreManager

            if (winner == null || s > bestScore)
            {
                winner = p;
                bestScore = s;
            }
        }

        if (winner == null || targetCamera == null)
            yield break;

        // 3) movemos la cámara hacia el ganador
        Vector3 startPos = targetCamera.transform.position;
        Vector3 targetPos = winner.character.position + cameraOffset;
        targetPos.z = startPos.z; // mantener la misma Z

        float t = 0f;
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / cameraMoveDuration);
            targetCamera.transform.position = Vector3.Lerp(startPos, targetPos, f);
            yield return null;
        }

        // 4) activamos el "WINS" del ganador
        if (winner.winsObject != null)
            winner.winsObject.SetActive(true);
    }
}
