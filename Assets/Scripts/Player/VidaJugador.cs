using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración de vidas")]
    public int maxImpactos = 3;
    private int impactosRecibidos = 0;

    [Tooltip("Arrastra aquí los objetos de UI que representan las vidas (corazones, etc.)")]
    public List<GameObject> iconosVida;

    // Evento que otros scripts pueden escuchar
    public delegate void RecibirDañoHandler();
    public event RecibirDañoHandler OnRecibirDaño;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Jugador detectó trigger con: {other.gameObject.name}");

        Bullet bala = other.GetComponent<Bullet>();
        if (bala != null && bala.esDelEnemigo)
        {
            impactosRecibidos++;
            Debug.Log($"Impactos recibidos por el jugador: {impactosRecibidos}");

            bala.gameObject.SetActive(false); // Desactiva la bala

            ActualizarUIVidas();

            // Emitir evento de daño
            OnRecibirDaño?.Invoke();

            if (impactosRecibidos >= maxImpactos)
            {
                Debug.Log("Jugador murió, reiniciando nivel...");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void ActualizarUIVidas()
    {
        int vidasRestantes = maxImpactos - impactosRecibidos;

        for (int i = 0; i < iconosVida.Count; i++)
        {
            iconosVida[i].SetActive(i < vidasRestantes);
        }
    }
}
