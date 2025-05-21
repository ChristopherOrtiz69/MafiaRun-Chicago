using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocidad = 20f;
    public float vida = 2f;
    public bool esDelEnemigo = false;

    private Vector2 direccion;
    private float tiempoDesactivacion;

    private bool impactoRegistrado = false;

    public void Disparar(Vector2 direccionDiscreta)
    {
        direccion = direccionDiscreta.normalized;
        tiempoDesactivacion = Time.time + vida;
        impactoRegistrado = false; // resetear impacto al disparar

        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        gameObject.SetActive(true);
    }

    public void DispararEnDireccion(Vector2 direccionFija)
    {
        direccion = direccionFija.normalized;
        tiempoDesactivacion = Time.time + vida;
        impactoRegistrado = false; // resetear impacto al disparar

        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        if (Time.time > tiempoDesactivacion)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (impactoRegistrado) return;

        if (esDelEnemigo && other.CompareTag("Player"))
        {
            impactoRegistrado = true;
            // No desactiva la bala aquí para que el otro script pueda contar el impacto
        }

        if (!esDelEnemigo && other.CompareTag("Enemy"))
        {
            impactoRegistrado = true;
            // Igual no desactiva aquí
        }
    }
}
