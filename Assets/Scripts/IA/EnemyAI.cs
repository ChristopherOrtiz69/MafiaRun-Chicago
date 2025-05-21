using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detección")]
    public Transform objetivo;
    public float rangoDeteccion = 5f;
    public float rangoDisparo = 3f;

    [Header("Movimiento")]
    public float velocidad = 2f;

    [Header("Distancia de detención antes de disparar")]
    public float distanciaDetencion = 1.5f; // Nueva variable para definir cuando dejar de avanzar

    [Header("Ataque")]
    public Transform puntoDisparo;
    public float fireRate = 1f;

    private float proximoDisparo;

    [Header("Bala")]
    public GameObject prefabBala; // Prefab único para este enemigo

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
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float direccionMovimiento = Mathf.Sign(objetivo.position.x - transform.position.x);

        Vector2 origenCheck = (Vector2)groundCheck.position;
        Vector2 direccionCheck = Vector2.down;
        Vector2 origenDesplazado = origenCheck + Vector2.right * direccionMovimiento * 0.1f;

        bool haySueloAdelante = Physics2D.Raycast(origenDesplazado, direccionCheck, checkDistance, oneWayPlatformLayer);

        if (!haySueloAdelante)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            if (distancia > rangoDisparo)
            {
                // Aquí comprobamos si la distancia es mayor que la distancia de detención para moverse
                if (distancia > distanciaDetencion)
                {
                    rb.velocity = new Vector2(direccionMovimiento * velocidad, rb.velocity.y);

                    Vector3 escala = transform.localScale;
                    escala.x = Mathf.Abs(escalaOriginal.x) * direccionMovimiento;
                    transform.localScale = escala;
                }
                else
                {
                    // Si está dentro de la distancia de detención, se detiene
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else
            {
                // Dentro de rango de disparo, se detiene y dispara
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
        if (puntoDisparo != null && prefabBala != null && BulletPool.Instance != null)
        {
            GameObject bala = BulletPool.Instance.ObtenerBala(prefabBala);
            bala.transform.position = puntoDisparo.position;
            bala.transform.rotation = Quaternion.identity;
            bala.SetActive(true);

            Vector2 direccion = (objetivo.position - transform.position).normalized;
            Bullet bulletScript = bala.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.DispararEnDireccion(direccion);
                bulletScript.esDelEnemigo = true;
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
