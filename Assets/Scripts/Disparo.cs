using UnityEngine;

public class Disparo : MonoBehaviour
{
    public Transform puntoDisparo;
    public Transform armaHolder; // punto donde se posiciona el arma visual
    public Weapon armaActual;
    private float tiempoProximoDisparo;

    private Animator animator;
    private GameObject armaVisualInstanciada;

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
        if (armaActual == null || armaActual.bulletPrefab == null) return;

        GameObject bala = Instantiate(armaActual.bulletPrefab, puntoDisparo.position, Quaternion.identity);

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = (mousePos - (Vector2)puntoDisparo.position).normalized;

        Vector2 direccionFinal;

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            direccionFinal = direccion.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            direccionFinal = direccion.y > 0 ? Vector2.up : Vector2.down;
        }

        bala.GetComponent<Bullet>().DispararEnDireccion(direccionFinal);
    }

    public void CambiarArma(Weapon nuevaArma)
    {
        armaActual = nuevaArma;

        if (armaVisualInstanciada != null)
            Destroy(armaVisualInstanciada);

        if (armaActual.prefabVisualArma != null && armaHolder != null)
        {
            armaVisualInstanciada = Instantiate(armaActual.prefabVisualArma, armaHolder.position, armaHolder.rotation, armaHolder);

            // Asignar pivotes desde hijos del prefab instanciado
            RotarArma rotador = armaVisualInstanciada.GetComponent<RotarArma>();
            if (rotador != null)
            {
                Transform pivotRight = armaVisualInstanciada.transform.Find("PivotRight");
                Transform pivotLeft = armaVisualInstanciada.transform.Find("PivotLeft");

                if (pivotRight != null && pivotLeft != null)
                {
                    rotador.pivotRight = pivotRight;
                    rotador.pivotLeft = pivotLeft;
                }
                else
                {
                    Debug.LogWarning("No se encontraron los pivots en el arma visual.");
                }
            }
        }
    }

}
