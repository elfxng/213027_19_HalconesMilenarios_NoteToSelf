using UnityEngine;

public class ShowRestartButton : MonoBehaviour
{
    [Header("UI")]
    public GameObject restartButton;   // botón de reinicio
    public GameObject winsObject;      // objeto que muestra el WINS

    private bool activated = false;

    void Start()
    {
        if (restartButton != null)
            restartButton.SetActive(false); // ocultar al inicio
    }

    void Update()
    {
        // Si WINS está activo y aún no mostramos el botón
        if (!activated && winsObject != null && winsObject.activeSelf)
        {
            activated = true;
            restartButton.SetActive(true); // mostrar el botón
        }
    }
}
