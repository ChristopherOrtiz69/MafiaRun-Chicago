using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Configuración Movimiento")]
    [SerializeField] private MovimientoConfig movimientoConfig;

    [Header("Configuración Salto")]
    [SerializeField] private SaltoConfig saltoConfig;

    [Header("Componentes")]
    [SerializeField] private GroundCheck groundCheckComponent;
    [SerializeField] private GravedadHandler gravedadHandler;
    [SerializeField] private PlataformaDrop plataformaDrop;
    [SerializeField] private Animator animator;

    private IInputHandler inputHandler;
    private Rigidbody2D rb;
    private int saltosRestantes;
    private float contadorCoyote;
    private float velocidadActual;
    private bool enSuelo;
    private int playerLayer;
    private bool puedeMoverse = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        inputHandler = GetComponent<IInputHandler>();
        if (inputHandler == null)
            Debug.LogError("No se encontró un componente que implemente IInputHandler");

        if (saltoConfig == null)
            Debug.LogError("Falta asignar saltoConfig en el inspector.");
        if (movimientoConfig == null)
            Debug.LogError("Falta asignar movimientoConfig en el inspector.");
    }

    void Start()
    {
        playerLayer = gameObject.layer;
        saltosRestantes = saltoConfig.SaltosMaximos;
        rb.gravityScale = saltoConfig.GravedadNormal;
        contadorCoyote = 0f;
    }

    void Update()
    {
        if (!puedeMoverse || inputHandler == null) return;

        enSuelo = groundCheckComponent.EstaEnSuelo();

        if (enSuelo)
        {
            saltosRestantes = saltoConfig.SaltosMaximos;
            contadorCoyote = saltoConfig.TiempoCoyote;
        }
        else
        {
            contadorCoyote -= Time.deltaTime;
        }

        float inputX = inputHandler.GetHorizontal();
        float aceleracionActual = inputX > 0 ? movimientoConfig.AceleracionAdelante : movimientoConfig.AceleracionAtras;

        if (inputX != 0)
        {
            float objetivo = inputX * movimientoConfig.VelocidadMaxima;
            velocidadActual = Mathf.Lerp(velocidadActual, objetivo, aceleracionActual * Time.deltaTime);
        }
        else
        {
            velocidadActual = Mathf.Lerp(velocidadActual, 0, movimientoConfig.Desaceleracion * Time.deltaTime);
        }

        rb.velocity = new Vector2(velocidadActual, rb.velocity.y);

        if (inputHandler.GetJump() && (saltosRestantes > 0 || contadorCoyote > 0f))
        {
            rb.velocity = new Vector2(rb.velocity.x, saltoConfig.FuerzaSalto);
            if (!enSuelo) saltosRestantes--;
            contadorCoyote = 0f;
        }

        if (inputHandler.GetDropDown() && enSuelo)
        {
            plataformaDrop.Iniciar();
        }

        gravedadHandler.Ajustar(rb);

        animator.SetFloat("Speed", Mathf.Abs(velocidadActual));
        animator.SetBool("Grounded", enSuelo);
    }

    public void DesactivarMovimiento()
    {
        puedeMoverse = false;
        rb.velocity = Vector2.zero;
        animator.SetFloat("Speed", 0);
    }

    public void ActivarMovimiento()
    {
        puedeMoverse = true;
    }
}
