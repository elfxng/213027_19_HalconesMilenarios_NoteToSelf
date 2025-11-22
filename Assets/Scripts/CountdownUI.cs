using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    public Image countdownImage;
    public Sprite threeSprite;
    public Sprite twoSprite;
    public Sprite oneSprite;
    public Sprite goSprite;
    public Sprite finishSprite;

    public float showTime = 1f;  // time between each number
    public AudioSource audioSource;
    public AudioClip startSound; // start sound

    public GameObject timer; // real game timer

    void Start()
    {
        timer.SetActive(false);  // Hide the stopwatch
        countdownImage.gameObject.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Sonido
        if (audioSource && startSound)
            audioSource.PlayOneShot(startSound);

        countdownImage.sprite = threeSprite;
        yield return new WaitForSeconds(showTime);

        countdownImage.sprite = twoSprite;
        yield return new WaitForSeconds(showTime);

        countdownImage.sprite = oneSprite;
        yield return new WaitForSeconds(showTime);

        countdownImage.sprite = goSprite;
        yield return new WaitForSeconds(showTime);

        countdownImage.gameObject.SetActive(false);
        
// ACTIVATE THE REAL STOPWATCH
        timer.SetActive(true);
    }

    public void ShowFinish()
    {
        countdownImage.sprite = finishSprite;
        countdownImage.gameObject.SetActive(true);
    }
}
