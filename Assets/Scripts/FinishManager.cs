using UnityEngine;
using System.Collections;

public class FinishManager : MonoBehaviour
{
    [Header("Finish UI")]
    public GameObject finishPanel;   // FINISH panel/image on the Canvas
    public float displayTime = 1.5f; // seconds for FINISH to disappear

    private bool started = false;

    void Start()
    {
        // make sure that it is turned off at startup
        if (finishPanel != null)
            finishPanel.SetActive(false);
    }

    void Update()
    {
        // when time runs out and we haven't shown FINISH yet
        if (!started && Timer.IsTimeUp)
        {
            started = true;
            StartCoroutine(ShowFinishRoutine());
        }
    }

    IEnumerator ShowFinishRoutine()
    {
        if (finishPanel != null)
            finishPanel.SetActive(true);     // show FINISH

        yield return new WaitForSeconds(displayTime);

        if (finishPanel != null)
            finishPanel.SetActive(false);    // hide FINISH
    }
}
