using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [Header("Finish UI")]
    public GameObject finishObject;    // el objeto FINISH del Canvas

    private bool shown = false;

    void Start()
    {
        // asegurarnos de que empieza apagado
        if (finishObject != null)
            finishObject.SetActive(false);
    }

    void Update()
    {
        if (shown) return;

        // esperamos a que el Timer diga que se acabó
        if (Timer.IsTimeUp)
        {
            shown = true;
            ShowFinish();
        }
    }

    void ShowFinish()
    {
        if (finishObject != null)
        {
            finishObject.SetActive(true);
            Debug.Log("FINISH mostrado");
        }
    }
}
