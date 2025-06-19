using UnityEngine;

public class GravedadHandler : MonoBehaviour
{
    [SerializeField] private float gravedadNormal = 1f;
    [SerializeField] private float gravedadCaida = 2.5f;

    public void Ajustar(Rigidbody2D rb)
    {
        if (rb.velocity.y < 0)
            rb.gravityScale = gravedadCaida;
        else
            rb.gravityScale = gravedadNormal;
    }
}
