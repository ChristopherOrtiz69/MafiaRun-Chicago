using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detección")]
    public Transform objetivo;
    public float rangoDeteccion = 5f;
    public float rangoDisparo = 3f;

    [Header("Movimiento")]
    public float velocidad = 2f;

    [Header("Ataque")]
    public Transform puntoDisparo;
    public float fireRate = 1f;
    private float proximoDisparo;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkDistance = 0.2f;
    public LayerMask oneWayPlatformLayer;

    private Rigidbody2D rb;
    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        if (objetivo == null) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);
        if (distancia > rangoDeteccion)
        {
            // No está en rango, no se mueve ni dispara
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // Dirección horizontal hacia el jugador
        float direccionMovimiento = Mathf.Sign(objetivo.position.x - transform.position.x);

        // Checar si hay suelo en la dirección que va a moverse para no caerse
        Vector2 origenCheck = (Vector2)groundCheck.position;
        Vector2 direccionCheck = Vector2.down;
        Vector2 origenDesplazado = origenCheck + Vector2.right * direccionMovimiento * 0.1f;

        bool haySueloAdelante = Physics2D.Raycast(origenDesplazado, direccionCheck, checkDistance, oneWayPlatformLayer);

        if (!haySueloAdelante)
        {
            // No hay suelo adelante, se detiene para no caer
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            if (distancia > rangoDisparo)
            {
                // Mover hacia el jugador
                rb.velocity = new Vector2(direccionMovimiento * velocidad, rb.velocity.y);

                // Ajustar escala para mirar al jugador
                Vector3 escala = transform.localScale;
                escala.x = Mathf.Abs(escalaOriginal.x) * direccionMovimiento;
                transform.localScale = escala;
            }
            else
            {
                // Está en rango de disparo, se detiene y dispara
                rb.velocity = new Vector2(0, rb.velocity.y);

                if (Time.time >= proximoDisparo)
                {
                    Disparar();
                    proximoDisparo = Time.time + fireRate;
                }
            }
        }
    }

    void Disparar()
    {
        if (puntoDisparo != null && BulletPool.Instance != null)
        {
            GameObject bala = BulletPool.Instance.ObtenerBala();
            bala.transform.position = puntoDisparo.position;
            bala.transform.rotation = Quaternion.identity;
            bala.SetActive(true);

            Vector2 direccion = (objetivo.position - transform.position).normalized;
            Bullet bulletScript = bala.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.DispararEnDireccion(direccion);
                bulletScript.esDelEnemigo = true; // Marca la bala como enemiga
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;

            Vector2 origenCheck = (Vector2)groundCheck.position;
            Gizmos.DrawLine(origenCheck, origenCheck + Vector2.down * checkDistance);
        }
    }
}
