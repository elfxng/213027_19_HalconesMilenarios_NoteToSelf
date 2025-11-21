using System.Collections;
using UnityEngine;

public class Stompable : MonoBehaviour
{
    [Header("Efecto de pisotón")]
    public float stunDuration = 3f;    // tiempo aturdido
    public float shrinkScale = 0.5f;   // tamaño mientras está pisado
    public float blinkSpeed = 0.15f;   // velocidad del parpadeo

    [Tooltip("Scripts de movimiento/salto que se desactivan mientras está aturdido")]
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

        // Hacerse pequeño
        transform.localScale = originalScale * shrinkScale;

        // Desactivar scripts de movimiento
        foreach (var s in movementScripts)
            if (s != null) s.enabled = false;

        // Iniciar parpadeo
        StartCoroutine(BlinkRoutine());

        // Esperar el tiempo de stun
        yield return new WaitForSeconds(stunDuration);

        // Volver a tamaño normal
        transform.localScale = originalScale;

        // Reactivar scripts de movimiento
        foreach (var s in movementScripts)
            if (s != null) s.enabled = true;

        stunned = false;
    }

    private IEnumerator BlinkRoutine()
    {
        float time = 0f;

        while (stunned)
        {
            // Alternar visibilidad
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkSpeed);
            time += blinkSpeed;
        }

        // Asegurarse de dejar visible cuando termine
        spriteRenderer.enabled = true;
    }
}
