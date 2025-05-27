using UnityEngine;
using Cinemachine;

public class MachineGunShoot : MonoBehaviour
{
    [Header("Configuración de disparo")]
    public Transform puntoDisparo;
    public Weapon armaActual;

    [Header("Sacudido de cámara")]
    public CinemachineImpulseSource impulseSource;

    [Header("Referencias")]
    public GameObject objetoConAnimator; // ⬅️ El GameObject del jugador (para pausar movimiento)
    public GameObject animatorDisparoObject; // ⬅️ El GameObject que tiene el Animator para la animación de disparo

    private float tiempoProximoDisparo;
    private Camera camaraPrincipal;

    private bool puedeDisparar = false;
    private GameObject jugador;
    private PlayerMovement2D scriptMovimientoJugador;
    private Rigidbody2D rbJugador;
    private Animator animatorDisparo; // ⬅️ Animator que controla la animación de disparo

    void Start()
    {
        camaraPrincipal = Camera.main;

        if (animatorDisparoObject != null)
        {
            animatorDisparo = animatorDisparoObject.GetComponent<Animator>();
        }

        if (objetoConAnimator != null)
        {
            // Solo desactivamos animaciones de movimiento aquí
        }
    }

    void Update()
    {
        if (jugador != null && Input.GetKeyDown(KeyCode.T))
        {
            if (scriptMovimientoJugador != null)
                scriptMovimientoJugador.enabled = true;

            if (objetoConAnimator != null)
            {
                Animator animJugador = objetoConAnimator.GetComponent<Animator>();
                if (animJugador != null)
                    animJugador.enabled = true;
            }

            puedeDisparar = false;

            if (animatorDisparo != null)
                animatorDisparo.SetBool("Disparando", false);
        }

        if (!puedeDisparar || armaActual == null) return;

        bool presionandoDisparo = Input.GetKey(KeyCode.K);

        if (animatorDisparo != null)
            animatorDisparo.SetBool("Disparando", presionandoDisparo);

        if (presionandoDisparo && Time.time >= tiempoProximoDisparo)
        {
            Vector2 direccionMouse = ObtenerDireccionHaciaMouse();
            Disparar(direccionMouse);
            tiempoProximoDisparo = Time.time + armaActual.fireRate;
        }
    }

    Vector2 ObtenerDireccionHaciaMouse()
    {
        Vector3 posicionMouse = Input.mousePosition;
        Vector3 posicionMundo = camaraPrincipal.ScreenToWorldPoint(posicionMouse);
        Vector2 direccion = (posicionMundo - puntoDisparo.position);
        return direccion.normalized;
    }

    void Disparar(Vector2 direccion)
    {
        GameObject bala = BulletPool.Instance.ObtenerBala(armaActual.bulletPrefab);

        if (bala == null)
        {
            Debug.Log("No hay balas disponibles en el pool.");
            return;
        }

        bala.transform.position = puntoDisparo.position;

        Bullet bulletScript = bala.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.esDelEnemigo = false;
            bulletScript.DispararEnDireccion(direccion);
        }

        bala.SetActive(true);

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("No se asignó CinemachineImpulseSource.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Jugador detectó trigger con Ametra");

            jugador = other.gameObject;
            scriptMovimientoJugador = jugador.GetComponent<PlayerMovement2D>();
            rbJugador = jugador.GetComponent<Rigidbody2D>();

            // Detener movimiento del jugador
            if (rbJugador != null)
                rbJugador.velocity = Vector2.zero;

            if (scriptMovimientoJugador != null)
                scriptMovimientoJugador.enabled = false;

            if (objetoConAnimator != null)
            {
                Animator animJugador = objetoConAnimator.GetComponent<Animator>();
                if (animJugador != null)
                    animJugador.enabled = false;
            }

            puedeDisparar = true;
        }
    }
}
