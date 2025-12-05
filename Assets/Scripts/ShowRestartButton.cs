using UnityEngine;

public class ShowRestartButton : MonoBehaviour
{
    [Header("UI")]
    public GameObject restartButton;   // reset button
    public GameObject winsObject;      // object that displays the WINS

    private bool activated = false;

    void Start()
    {
        if (restartButton != null)
            restartButton.SetActive(false); // hide at start
    }

    void Update()
    {
        // If WINDOWS is active and we haven't yet displayed the button
        if (!activated && winsObject != null && winsObject.activeSelf)
        {
            activated = true;
            restartButton.SetActive(true); // show button
        }
    }
}
