using UnityEngine;
using System.Collections;

public class HelicopterAI : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;

    [Header("Rangos")]
    public float rangoDeteccion = 10f;
    public float rangoDisparo = 6f;
    public float distanciaStop = 0.3f; 

    [Header("Movimiento")]
    public float fixedHeight = 5f;
    public float velocidadSeguimiento = 3f;

    [Header("Ataque")]
    public Transform puntoDisparo;
    public GameObject prefabBala;
    public float fireRate = 1f;

    private float proximoDisparo;
    private bool estaEsperando = false;

    void Update()
    {
        if (objetivo == null) return;

        float distanciaJugador = Vector2.Distance(transform.position, objetivo.position);

        if (distanciaJugador <= rangoDeteccion)
        {
            Vector3 objetivoPos = new Vector3(objetivo.position.x, fixedHeight, transform.position.z);
            float distanciaHorizontal = Mathf.Abs(transform.position.x - objetivo.position.x);

            
            if (distanciaHorizontal <= distanciaStop)
            {
                if (!estaEsperando)
                    StartCoroutine(EsperarAntesDeMover());
            }
            else if (!estaEsperando)
            {
                
                transform.position = Vector3.MoveTowards(transform.position, objetivoPos, velocidadSeguimiento * Time.deltaTime);
            }

            // Disparar
            if (distanciaJugador <= rangoDisparo && Time.time >= proximoDisparo)
            {
                Disparar();
                proximoDisparo = Time.time + fireRate;
            }
        }
    }

    IEnumerator EsperarAntesDeMover()
    {
        estaEsperando = true;
        yield return new WaitForSeconds(1.5f);
        estaEsperando = false;
    }

    void Disparar()
    {
        if (puntoDisparo != null && prefabBala != null && BulletPool.Instance != null)
        {
            GameObject bala = BulletPool.Instance.ObtenerBala(prefabBala);
            bala.transform.position = puntoDisparo.position;
            bala.transform.rotation = Quaternion.identity;
            bala.SetActive(true);

            Vector2 centroObjetivo = objetivo.position;

            Collider2D collider = objetivo.GetComponent<Collider2D>();
            if (collider != null)
            {
                centroObjetivo = collider.bounds.center;
            }

            Vector2 direccion = (centroObjetivo - (Vector2)transform.position).normalized;

            Bullet bulletScript = bala.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Disparar(direccion);
                bulletScript.esDelEnemigo = true;
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        if (objetivo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rangoDisparo);
        }
    }
}
