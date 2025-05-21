using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaJugador : MonoBehaviour
{
    public int maxImpactos = 3;
    private int impactosRecibidos = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Jugador detectó trigger con: {other.gameObject.name}");

        Bullet bala = other.GetComponent<Bullet>();
        if (bala != null && bala.esDelEnemigo)
        {
            impactosRecibidos++;
            Debug.Log($"Impactos recibidos por el jugador: {impactosRecibidos}");

            bala.gameObject.SetActive(false); // Desactiva la bala

            if (impactosRecibidos >= maxImpactos)
            {
                Debug.Log("Jugador murió, reiniciando nivel...");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
