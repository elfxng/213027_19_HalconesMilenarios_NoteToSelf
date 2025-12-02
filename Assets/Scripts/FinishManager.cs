using UnityEngine;
using System.Collections;

public class FinishManager : MonoBehaviour
{
    [Header("Finish UI")]
    public GameObject finishPanel;   // panel/imagen FINISH en el Canvas
    public float displayTime = 1.5f; // cuántos segundos se ve FINISH

    private bool started = false;

    void Start()
    {
        // asegurarnos que inicia apagado
        if (finishPanel != null)
            finishPanel.SetActive(false);
    }

    void Update()
    {
        // cuando se acabe el tiempo y aún no hemos mostrado FINISH
        if (!started && Timer.IsTimeUp)
        {
            started = true;
            StartCoroutine(ShowFinishRoutine());
        }
    }

    IEnumerator ShowFinishRoutine()
    {
        if (finishPanel != null)
            finishPanel.SetActive(true);     // mostrar FINISH

        yield return new WaitForSeconds(displayTime);

        if (finishPanel != null)
            finishPanel.SetActive(false);    // ocultar FINISH
    }
}
