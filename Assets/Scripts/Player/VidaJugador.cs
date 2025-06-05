using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections.Generic;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración de vidas")]
    public int maxImpactos = 3;
    private int impactosRecibidos = 0;

    [Tooltip("Arrastra aquí los objetos de UI que representan las vidas (corazones, etc.)")]
    public List<GameObject> iconosVida;

    [Header("Panel de Game Over")]
    public GameObject panelGameOver;

    
    public delegate void RecibirDañoHandler();
    public event RecibirDañoHandler OnRecibirDaño;

    private void Start()
    {
        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        Time.timeScale = 1f; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Jugador detectó trigger con: {other.gameObject.name}");

        Bullet bala = other.GetComponent<Bullet>();
        if (bala != null && bala.esDelEnemigo)
        {
            impactosRecibidos++;
            Debug.Log($"Impactos recibidos por el jugador: {impactosRecibidos}");

            bala.gameObject.SetActive(false);

            ActualizarUIVidas();

            OnRecibirDaño?.Invoke();

            if (impactosRecibidos >= maxImpactos)
            {
                Debug.Log("Jugador murió, activando Game Over...");
                Time.timeScale = 0f; 

                if (panelGameOver != null)
                    panelGameOver.SetActive(true);
                else
                    Debug.LogWarning("No se asignó un panel de Game Over en el inspector.");
            }
        }
    }

    void ActualizarUIVidas()
    {
        int vidasRestantes = maxImpactos - impactosRecibidos;

        for (int i = 0; i < iconosVida.Count; i++)
        {
            Image imagen = iconosVida[i].GetComponent<Image>();
            if (imagen != null)
            {
                imagen.color = (i < vidasRestantes) ? Color.white : new Color(1, 1, 1, 0); // oculta visualmente
            }
        }
    }

    // Llama esta función desde un botón del panel de Game Over
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
