using UnityEngine;
using Cinemachine;

public class MachineGunShoot : MonoBehaviour
{
    [Header("Configuración de disparo")]
    public Transform puntoDisparo;
    public Weapon armaActual;

    [Header("Indicador de dirección")]
    public GameObject flechaDireccion;

    [Header("Sacudido de cámara")]
    public CinemachineImpulseSource impulseSource;

    [Header("Referencias")]
    public GameObject objetoConAnimator;
    public GameObject animatorDisparoObject;

    [Header("Audio")]
    public AudioSource audioSourceDisparo;
    public AudioClip clipDisparo;

    [Header("Rotación de disparo")]
    public float anguloMin = -90f;
    public float anguloMax = 90f;
    public float velocidadRotacion = 90f;

    [Header("UI")]
    public GameObject mensajeUso; // Texto que aparece cuando el jugador se acerca

    private float anguloActual = 0f;
    private float tiempoProximoDisparo;
    private Camera camaraPrincipal;

    private bool puedeDisparar = false;
    private GameObject jugador;
    private PlayerMovement2D scriptMovimientoJugador;
    private Rigidbody2D rbJugador;
    private Animator animatorDisparo;

    private bool jugadorEnContacto = false;

    void Start()
    {
        camaraPrincipal = Camera.main;

        if (animatorDisparoObject != null)
            animatorDisparo = animatorDisparoObject.GetComponent<Animator>();

        if (mensajeUso != null)
            mensajeUso.SetActive(true);
    }

    void Update()
    {
        if (jugadorEnContacto && Input.GetKeyDown(KeyCode.T))
        {
            AlternarEstadoDisparo();
        }

        if (!puedeDisparar || armaActual == null)
            return;

        float inputVertical = Input.GetAxisRaw("Vertical");
        anguloActual += inputVertical * velocidadRotacion * Time.deltaTime;
        anguloActual = Mathf.Clamp(anguloActual, anguloMin, anguloMax);

        puntoDisparo.localRotation = Quaternion.Euler(0, 0, anguloActual);

        if (flechaDireccion != null)
        {
            flechaDireccion.transform.position = puntoDisparo.position;
            flechaDireccion.transform.rotation = puntoDisparo.rotation;
        }

        bool presionandoDisparo = Input.GetKey(KeyCode.K);

        if (animatorDisparo != null)
            animatorDisparo.SetBool("Disparando", presionandoDisparo);

        if (presionandoDisparo)
        {
            if (Time.time >= tiempoProximoDisparo)
            {
                Vector2 direccion = puntoDisparo.right;
                Disparar(direccion);
                tiempoProximoDisparo = Time.time + armaActual.fireRate;
            }

            if (audioSourceDisparo != null && !audioSourceDisparo.isPlaying)
            {
                audioSourceDisparo.clip = clipDisparo;
                audioSourceDisparo.loop = true;
                audioSourceDisparo.Play();
            }
        }
        else
        {
            if (audioSourceDisparo != null && audioSourceDisparo.isPlaying)
            {
                audioSourceDisparo.Stop();
                audioSourceDisparo.loop = false;
            }
        }
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
            bulletScript.Disparar(direccion);
        }

        bala.SetActive(true);

        if (impulseSource != null)
            impulseSource.GenerateImpulse();
    }

    private void AlternarEstadoDisparo()
    {
        puedeDisparar = !puedeDisparar;

        if (puedeDisparar)
        {
            if (jugador != null)
            {
                scriptMovimientoJugador = jugador.GetComponent<PlayerMovement2D>();
                rbJugador = jugador.GetComponent<Rigidbody2D>();

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
            }
        }
        else
        {
            if (scriptMovimientoJugador != null)
                scriptMovimientoJugador.enabled = true;

            if (objetoConAnimator != null)
            {
                Animator animJugador = objetoConAnimator.GetComponent<Animator>();
                if (animJugador != null)
                    animJugador.enabled = true;
            }

            if (animatorDisparo != null)
                animatorDisparo.SetBool("Disparando", false);
        }

        if (mensajeUso != null)
            mensajeUso.SetActive(!puedeDisparar);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorEnContacto = true;
            jugador = collision.gameObject;

            if (mensajeUso != null && !puedeDisparar)
                mensajeUso.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorEnContacto = false;

            if (mensajeUso != null)
                mensajeUso.SetActive(false);
        }
    }
}
