using UnityEngine;

public class FinishUI : MonoBehaviour
{
    public GameObject finishImage;   // reference to the UI Image object
    public Animator finishAnimator;  // animator for the finish image
    public string showTriggerName = "Show"; // trigger name in animator (if you use one)

    private bool played = false;

    void Start()
    {
        // make sure it starts hidden
        if (finishImage != null)
            finishImage.SetActive(false);
    }

    void Update()
    {
        if (!played && Timer.IsTimeUp)
        {
            played = true;

            // show image
            if (finishImage != null)
                finishImage.SetActive(true);

            // play animation
            if (finishAnimator != null && !string.IsNullOrEmpty(showTriggerName))
            {
                finishAnimator.SetTrigger(showTriggerName);
            }
        }
    }
}
