using UnityEngine;

public class SpriteFlipTowardsTarget : MonoBehaviour
{
    [Header("Objetivo que debe seguir (ej: jugador)")]
    public Transform objetivo;

    void Update()
    {
        if (objetivo == null) return;

        // Dirección del objetivo al arma
        Vector2 direccion = objetivo.position - transform.position;

        // Ángulo en 2D (solo en el plano Z)
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Rotar solo en el eje Z (para mantener sprite plano y evitar distorsión)
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }
}

