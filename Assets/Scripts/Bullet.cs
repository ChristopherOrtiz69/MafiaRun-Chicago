using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocidad = 20f;
    public float vida = 2f;

    private Vector2 direccion;
    private float tiempoDesactivacion;

    
    public void Disparar(Vector2 direccionDiscreta)
    {
        direccion = direccionDiscreta.normalized;
        tiempoDesactivacion = Time.time + vida;

        // ROTAR la bala para que mire en la dirección de disparo
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        gameObject.SetActive(true);
    }
    public void DispararEnDireccion(Vector2 direccionFija)
    {
        direccion = direccionFija.normalized;
        tiempoDesactivacion = Time.time + vida;

        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        gameObject.SetActive(true);
    }

    void Update()
    {
        // Mover la bala en la dirección establecida
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);

        // Desactivarla si ya pasó su "vida útil"
        if (Time.time > tiempoDesactivacion)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Desactivar al colisionar con algo
        //gameObject.SetActive(false);
    }
}
