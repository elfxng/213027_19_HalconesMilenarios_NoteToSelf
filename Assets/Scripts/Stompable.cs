using System.Collections;
using UnityEngine;

public class Stompable : MonoBehaviour
{
    [Header("Stomp effect")]
    public float stunDuration = 3f;     // time stunned
    public float shrinkScale = 0.5f;    // how small the character becomes
    public float blinkSpeed = 0.15f;    // blink frequency
    public float popUpOffsetY = 0.2f;   // how much to move up when returning to normal size

    [Tooltip("Movement scripts that should be disabled while stunned")]
    public MonoBehaviour[] movementScripts;

    private Vector3 originalScale;
    private bool stunned = false;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Stomp()
    {
        if (stunned) return;
        StartCoroutine(StompRoutine());
    }

    private IEnumerator StompRoutine()
    {
        stunned = true;

        // Make character smaller
        transform.localScale = originalScale * shrinkScale;

        // Disable movement scripts
        foreach (var s in movementScripts)
        {
            if (s != null) s.enabled = false;
        }

        // Start blinking
        StartCoroutine(BlinkRoutine());

        // Wait while stunned
        yield return new WaitForSeconds(stunDuration);

        // Restore normal size
        transform.localScale = originalScale;

        // Move character slightly up to avoid being inside the ground
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + popUpOffsetY,
            transform.position.z
        );

        // Re-enable movement scripts
        foreach (var s in movementScripts)
        {
            if (s != null) s.enabled = true;
        }

        stunned = false;
    }

    private IEnumerator BlinkRoutine()
    {
        if (spriteRenderer == null)
            yield break;

        while (stunned)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkSpeed);
        }

        // Make sure it ends visible
        spriteRenderer.enabled = true;
    }
}
