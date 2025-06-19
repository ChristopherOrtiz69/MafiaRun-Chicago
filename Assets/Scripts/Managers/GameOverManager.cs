using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Panel de Game Over")]
    public GameObject panelGameOver;

    [Header("Vida del jugador")]
    public VidaJugador vidaJugador;

    private void Start()
    {
        if (panelGameOver != null)
            panelGameOver.SetActive(false);

        Time.timeScale = 1f;

        if (vidaJugador != null)
        {
            vidaJugador.OnVidasAgotadas += ActivarGameOver;
        }
    }

    private void OnDestroy()
    {
        if (vidaJugador != null)
        {
            vidaJugador.OnVidasAgotadas -= ActivarGameOver;
        }
    }

    private void ActivarGameOver()
    {
        Time.timeScale = 0f;
        if (panelGameOver != null)
            panelGameOver.SetActive(true);
        else
            Debug.LogWarning("No se asignó un panel de Game Over en el inspector.");
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
