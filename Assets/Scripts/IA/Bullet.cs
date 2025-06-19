using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocidad = 20f;
    public float vida = 2f;
    public bool esDelEnemigo = false;

    private Vector2 direccion;
    private float tiempoDesactivacion;
    private bool impactoRegistrado = false;

    public void Disparar(Vector2 direccionDisparada)
    {
        direccion = direccionDisparada.normalized;
        tiempoDesactivacion = Time.time + vida;
        impactoRegistrado = false;

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        if (Time.time > tiempoDesactivacion)
        {
            BulletPool.Instance?.DevolverBala(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (impactoRegistrado) return;

        if (esDelEnemigo && other.CompareTag("Player"))
        {
            other.GetComponent<Health>()?.RecibirDaño(10f);
            other.GetComponent<VidaJugador>()?.RegistrarImpacto();

            impactoRegistrado = true;
            BulletPool.Instance?.DevolverBala(gameObject);
        }

        if (!esDelEnemigo && other.CompareTag("Enemy"))
        {
            Health saludEnemigo = other.GetComponent<Health>();
            if (saludEnemigo != null)
            {
                saludEnemigo.RecibirDaño(10f);
                impactoRegistrado = true;
                BulletPool.Instance?.DevolverBala(gameObject);
            }
        }
    }

    void OnDisable()
    {
        impactoRegistrado = false;
    }
}
