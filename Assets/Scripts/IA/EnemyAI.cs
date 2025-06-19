using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform objetivo;
    [SerializeField] private float rangoDeteccion = 5f;
    [SerializeField] private float rangoDisparo = 3f;
    [SerializeField] private float distanciaDetencion = 1.5f;
    [SerializeField] private bool seguirJugador = true;

    private EnemyMovement movement;
    private EnemyAttack attack;
    private EnemyHealth health;

    private Vector2 posEnemigo;
    private Vector2 posObjetivo;
    private float distancia;

    public Transform Objetivo => objetivo;

    public void SetObjetivo(Transform nuevoObjetivo)
    {
        objetivo = nuevoObjetivo;
    }

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyAttack>();
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (objetivo == null || !seguirJugador) return;

        posEnemigo = transform.position;
        posObjetivo = objetivo.position;
        distancia = (posObjetivo - posEnemigo).sqrMagnitude;

        float rangoDeteccionSqr = rangoDeteccion * rangoDeteccion;
        float rangoDisparoSqr = rangoDisparo * rangoDisparo;
        float distanciaDetencionSqr = distanciaDetencion * distanciaDetencion;

        if (distancia > rangoDeteccionSqr)
        {
            movement.Detener();
            return;
        }

        float direccionMovimiento = Mathf.Sign(posObjetivo.x - posEnemigo.x);

        if (movement.PuedeMoverseHacia(direccionMovimiento) && distancia > distanciaDetencionSqr)
            movement.Mover(direccionMovimiento);
        else
            movement.Detener();

        if (distancia <= rangoDisparoSqr)
            attack.IntentarDisparar(objetivo);
    }

    public void Morir()
    {
        health.Morir();
    }
}
