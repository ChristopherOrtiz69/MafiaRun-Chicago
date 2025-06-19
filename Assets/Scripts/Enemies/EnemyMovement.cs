using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask oneWayPlatformLayer;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float checkDistance = 0.2f;
    [SerializeField] private float distanciaChequeo = 0.6f;
    [SerializeField] private float velocidad = 2f;

    private Rigidbody2D rb;
    private Vector3 escalaOriginal;

    private RaycastHit2D[] resultadosRaycast = new RaycastHit2D[1];

    public Transform GroundCheck => groundCheck;
    public float CheckDistance => checkDistance;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale;
    }

    public bool PuedeMoverseHacia(float direccion)
    {
        Vector2 origenCheck = groundCheck.position;
        Vector2 origenDesplazado = origenCheck + Vector2.right * direccion * 0.1f;

        int hitSuelo = Physics2D.RaycastNonAlloc(origenDesplazado, Vector2.down, resultadosRaycast, checkDistance, oneWayPlatformLayer);
        if (hitSuelo == 0)
            return false;

        if (DetectarEnemigoLateral())
            return false;

        return true;
    }

    public void Mover(float direccion)
    {
        rb.velocity = new Vector2(direccion * velocidad, rb.velocity.y);

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Sign(direccion) * Mathf.Abs(escalaOriginal.x);
        transform.localScale = escala;
    }

    public void Detener()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    private bool DetectarEnemigoLateral()
    {
        float offsetHorizontal = 0.5f;
        Vector2 origenIzquierda = (Vector2)transform.position + Vector2.left * offsetHorizontal;
        Vector2 origenDerecha = (Vector2)transform.position + Vector2.right * offsetHorizontal;

        int hitsIzq = Physics2D.RaycastNonAlloc(origenIzquierda, Vector2.left, resultadosRaycast, distanciaChequeo, enemyLayer);
        int hitsDer = Physics2D.RaycastNonAlloc(origenDerecha, Vector2.right, resultadosRaycast, distanciaChequeo, enemyLayer);

        return hitsIzq > 0 || hitsDer > 0;
    }
}
