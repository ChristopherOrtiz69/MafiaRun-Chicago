using UnityEngine;
using Cinemachine;

public class Disparo : MonoBehaviour
{
    public Transform puntoDisparo;
    public Transform armaHolder;
    public Weapon armaActual;
    private float tiempoProximoDisparo;

    private Animator animator;
    private GameObject armaVisualInstanciada;

    public Transform pivoteContainer;

    [Header("Sacudido de cámara")]
    public CinemachineImpulseSource impulseSource;

    [Header("Opciones especiales de arma")]
    public bool esEscopeta = false;
    public float separacionAngulo = 10f;

    private Vector2 ultimaDireccion = Vector2.right;

    [Header("Audio de disparo")]
    public AudioSource audioSource;
    public AudioClip sonidoDisparo;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (armaActual != null)
            CambiarArma(armaActual);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) ultimaDireccion = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) ultimaDireccion = Vector2.down;
        else if (Input.GetKey(KeyCode.D)) ultimaDireccion = Vector2.right;
        else if (Input.GetKey(KeyCode.A)) ultimaDireccion = Vector2.left;

        bool presionandoDisparo = Input.GetKey(KeyCode.K);

        if (animator != null)
            animator.SetBool("Disparando", presionandoDisparo);

        if (presionandoDisparo && Time.time >= tiempoProximoDisparo)
        {
            Disparar(ultimaDireccion);
            tiempoProximoDisparo = Time.time + armaActual.fireRate;
        }
    }

    void Disparar(Vector2 direccion)
    {
        if (armaActual == null || armaActual.bulletPrefab == null || puntoDisparo == null) return;

        if (esEscopeta)
        {
            DispararBalaConAngulo(direccion, 0);
            DispararBalaConAngulo(direccion, separacionAngulo);
            DispararBalaConAngulo(direccion, -separacionAngulo);
        }
        else
        {
            DispararBalaConAngulo(direccion, 0);
        }

        if (audioSource != null && sonidoDisparo != null)
        {
            audioSource.PlayOneShot(sonidoDisparo);
        }
        else
        {
            Debug.LogWarning("AudioSource o sonidoDisparo no asignado.");
        }

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("No se asignó CinemachineImpulseSource en Disparo.cs");
        }
    }

    void DispararBalaConAngulo(Vector2 direccion, float angulo)
    {
        GameObject bala = BulletPool.Instance.ObtenerBala(armaActual.bulletPrefab);
        if (bala == null)
        {
            Debug.Log("No hay balas disponibles en el pool.");
            return;
        }

        bala.transform.position = puntoDisparo.position;

        Vector2 direccionRotada = Quaternion.Euler(0, 0, angulo) * direccion;

        Bullet bulletScript = bala.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.esDelEnemigo = false;
            bulletScript.Disparar(direccionRotada.normalized);
        }

        bala.SetActive(true);
    }

    public void CambiarArma(Weapon nuevaArma)
    {
        armaActual = nuevaArma;

        if (armaVisualInstanciada != null)
            Destroy(armaVisualInstanciada);

        if (armaActual.prefabVisualArma != null && armaHolder != null)
        {
            armaVisualInstanciada = Instantiate(armaActual.prefabVisualArma, armaHolder.position, armaHolder.rotation, armaHolder);

            RotarArma rotador = armaVisualInstanciada.GetComponent<RotarArma>();
            if (rotador != null && pivoteContainer != null)
            {
                rotador.pivotRight = pivoteContainer.Find("PivotRight");
                rotador.pivotLeft = pivoteContainer.Find("PivotLeft");
                rotador.pivotUp = pivoteContainer.Find("PivotUp");
                rotador.pivotDown = pivoteContainer.Find("PivotDown");

                if (rotador.pivotRight == null || rotador.pivotLeft == null || rotador.pivotUp == null || rotador.pivotDown == null)
                {
                    Debug.LogWarning("Uno o más pivotes no fueron encontrados en el Player.");
                }
            }
        }
    }
}
