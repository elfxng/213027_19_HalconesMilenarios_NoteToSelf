using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Movimiento")]
    public float fallSpeed = 2f;   // velocidad de caída
    public float destroyY = -6f;   // posición donde se destruye (fuera de pantalla)

    void Update()
    {
        // Mover la nota hacia abajo
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Si ya cayó más abajo de cierto punto, la destruimos
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}

