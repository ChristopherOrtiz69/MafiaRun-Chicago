using UnityEngine;

public class FlipX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Obtener el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("No se encontró un SpriteRenderer en el objeto.");
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            spriteRenderer.flipX = false;
        }
    }
}
