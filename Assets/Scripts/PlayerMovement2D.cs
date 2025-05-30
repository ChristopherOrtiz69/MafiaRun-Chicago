using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMaxima = 5f;
    public float aceleracionAdelante = 10f;
    public float aceleracionAtras = 5f;
    public float desaceleracion = 15f;

    [Header("Salto")]
    public float fuerzaSalto = 12f;

    [Header("Doble Salto")]
    public int saltosMaximos = 2;
    private int saltosRestantes;

    [Header("Gravedad y salto")]
    public float gravedadNormal = 1f;
    public float gravedadCaida = 2.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Coyote Time")]
    public float tiempoCoyote = 0.2f;
    private float contadorCoyote;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator animator;
    private float velocidadActual = 0f;
    private bool mirandoDerecha = true;
    private bool enSuelo;

    private int playerLayer;
    public string plataformaLayer = "OneWayPlatform";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();

        playerLayer = gameObject.layer;
        rb.gravityScale = gravedadNormal;
    }

    void Update()
    {
        // Detección de suelo y coyote time
        enSuelo = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (enSuelo)
        {
            saltosRestantes = saltosMaximos;
            contadorCoyote = tiempoCoyote;
        }
        else
        {
            contadorCoyote -= Time.deltaTime;
        }

        float inputX = Input.GetAxisRaw("Horizontal");

        float aceleracionActual = inputX > 0 ? aceleracionAdelante : aceleracionAtras;

        if (inputX != 0)
        {
            float objetivo = inputX * velocidadMaxima;
            velocidadActual = Mathf.Lerp(velocidadActual, objetivo, aceleracionActual * Time.deltaTime);
        }
        else
        {
            velocidadActual = Mathf.Lerp(velocidadActual, 0, desaceleracion * Time.deltaTime);
        }

        rb.velocity = new Vector2(velocidadActual, rb.velocity.y);

        // Salto con coyote time
        if (Input.GetButtonDown("Jump") && (saltosRestantes > 0 || contadorCoyote > 0f))
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

            if (!enSuelo)
                saltosRestantes--;

            contadorCoyote = 0f;
        }

        // Baja por plataformas
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetAxisRaw("Vertical") < -0.1f) && enSuelo)
        {
            StartCoroutine(DesactivarColisionTemporal());
        }

        AjustarGravedadSalto();

        // Animaciones
        animator.SetFloat("Speed", Mathf.Abs(velocidadActual));
        animator.SetBool("Grounded", enSuelo);
    }

    IEnumerator DesactivarColisionTemporal()
    {
        int plataformaLayerIndex = LayerMask.NameToLayer(plataformaLayer);
        Physics2D.IgnoreLayerCollision(playerLayer, plataformaLayerIndex, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(playerLayer, plataformaLayerIndex, false);
    }

    void AjustarGravedadSalto()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = gravedadCaida;
        }
        else
        {
            rb.gravityScale = gravedadNormal;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
