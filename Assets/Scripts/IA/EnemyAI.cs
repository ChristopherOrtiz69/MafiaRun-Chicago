using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detección")]
    public Transform objetivo;
    public float rangoDeteccion = 5f;
    public float rangoDisparo = 3f;

    [Header("Control de activación manual")]
    public bool seguirJugador = true;

    [Header("Movimiento")]
    public float velocidad = 2f;
    public float distanciaDetencion = 1.5f;

    [Header("Ataque")]
    public Transform puntoDisparo;
    public float fireRate = 1f;
    private float proximoDisparo;

    [Header("Bala")]
    public GameObject prefabBala;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkDistance = 0.2f;
    public LayerMask oneWayPlatformLayer;

    [Header("Detección lateral")]
    public float distanciaChequeo = 0.6f;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private Vector3 escalaOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale;
    }
    void Update()
    {
        if (objetivo == null || !seguirJugador) return;

        float distancia = Vector2.Distance(transform.position, objetivo.position);

        if (distancia > rangoDeteccion)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float direccionMovimiento = Mathf.Sign(objetivo.position.x - transform.position.x);

        // Comprobar suelo adelante
        Vector2 origenCheck = (Vector2)groundCheck.position;
        Vector2 origenDesplazado = origenCheck + Vector2.right * direccionMovimiento * 0.1f;

        bool haySueloAdelante = Physics2D.Raycast(origenDesplazado, Vector2.down, checkDistance, oneWayPlatformLayer);
        Debug.DrawLine(origenDesplazado, origenDesplazado + Vector2.down * checkDistance, Color.yellow);

        bool puedeMoverse = true;

        if (!haySueloAdelante)
        {
            Debug.Log($"{gameObject.name} se detiene por falta de suelo adelante");
            puedeMoverse = false;
        }

        // Detectar si hay enemigo adelante
        bool hayEnemigoAdelante = DetectarEnemigoLateral();
        if (hayEnemigoAdelante)
        {
            Debug.Log($"{gameObject.name} detecta a otro enemigo cerca y se detiene");
            puedeMoverse = false;
        }

        // Movimiento hacia el jugador
        if (puedeMoverse && distancia > distanciaDetencion)
        {
            rb.velocity = new Vector2(direccionMovimiento * velocidad, rb.velocity.y);

            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escalaOriginal.x) * direccionMovimiento;
            transform.localScale = escala;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Ataque (disparar aunque no se mueva)
        if (distancia <= rangoDisparo && Time.time >= proximoDisparo)
        {
            Disparar();
            proximoDisparo = Time.time + fireRate;
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

            Vector2 objetivoCentro;
            Collider2D colliderJugador = objetivo.GetComponent<Collider2D>();
            if (colliderJugador != null)
                objetivoCentro = colliderJugador.bounds.center;
            else
                objetivoCentro = objetivo.position;

            Vector2 direccion = (objetivoCentro - (Vector2)puntoDisparo.position).normalized;

            Bullet bulletScript = bala.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.DispararEnDireccion(direccion);
                bulletScript.esDelEnemigo = true;
            }
        }
    }

    bool DetectarEnemigoLateral()
    {
        float offsetHorizontal = 0.5f; 
        Vector2 origenIzquierda = (Vector2)transform.position + Vector2.left * offsetHorizontal;
        Vector2 origenDerecha = (Vector2)transform.position + Vector2.right * offsetHorizontal;

        RaycastHit2D hitIzquierda = Physics2D.Raycast(origenIzquierda, Vector2.left, distanciaChequeo, enemyLayer);
        RaycastHit2D hitDerecha = Physics2D.Raycast(origenDerecha, Vector2.right, distanciaChequeo, enemyLayer);

        Debug.DrawRay(origenIzquierda, Vector2.left * distanciaChequeo, Color.magenta);
        Debug.DrawRay(origenDerecha, Vector2.right * distanciaChequeo, Color.magenta);

        if ((hitIzquierda.collider != null && hitIzquierda.collider.gameObject != gameObject) ||
            (hitDerecha.collider != null && hitDerecha.collider.gameObject != gameObject))
        {
            return true;
        }

        return false;
    }

    public void Morir()
    {
        SpecialAbilityController controlador = FindObjectOfType<SpecialAbilityController>();
        if (controlador != null)
        {
            controlador.RegistrarEnemigoEliminado(gameObject);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Vector2 origenCheck = (Vector2)groundCheck.position;
            Gizmos.DrawLine(origenCheck, origenCheck + Vector2.down * checkDistance);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDisparo);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaDetencion);
    }
}
