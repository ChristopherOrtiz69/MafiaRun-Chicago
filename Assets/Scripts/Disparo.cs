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

    void Start()
    {
        animator = GetComponent<Animator>();
        if (armaActual != null)
            CambiarArma(armaActual);
    }

    void Update()
    {
        bool presionandoDisparo = Input.GetButton("Fire1");

        if (animator != null)
            animator.SetBool("Disparando", presionandoDisparo);

        if (presionandoDisparo && Time.time >= tiempoProximoDisparo)
        {
            Disparar();
            tiempoProximoDisparo = Time.time + armaActual.fireRate;
        }
    }

    void Disparar()
    {
        if (armaActual == null || armaActual.bulletPrefab == null || puntoDisparo == null) return;

        GameObject bala = BulletPool.Instance.ObtenerBala(armaActual.bulletPrefab); // ← CORREGIDO

        if (bala == null)
        {
            Debug.Log("No hay balas disponibles en el pool.");
            return;
        }

        bala.transform.position = puntoDisparo.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = (mousePos - (Vector2)puntoDisparo.position).normalized;

        Vector2 direccionFinal;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
            direccionFinal = direccion.x > 0 ? Vector2.right : Vector2.left;
        else
            direccionFinal = direccion.y > 0 ? Vector2.up : Vector2.down;

        Bullet bulletScript = bala.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.esDelEnemigo = false;
            bulletScript.DispararEnDireccion(direccionFinal);
        }

        bala.SetActive(true);

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("No se asignó CinemachineImpulseSource en Disparo.cs");
        }
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
