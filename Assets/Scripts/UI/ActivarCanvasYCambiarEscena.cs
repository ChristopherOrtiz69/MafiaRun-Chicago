using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivarCanvasYCambiarEscena : MonoBehaviour
{
    [Header("Objeto que se activará (por ejemplo, un Canvas)")]
    public GameObject objetoActivar;

    [Header("Índice de la escena a cargar al presionar Q")]
    public int indiceEscena = -1;

    private bool jugadorDentro = false;

    void Start()
    {
        if (objetoActivar != null)
        {
            objetoActivar.SetActive(false); // Asegura que inicie desactivado
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;

            if (objetoActivar != null)
                objetoActivar.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;

            if (objetoActivar != null)
                objetoActivar.SetActive(false);
        }
    }

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.Q))
        {
            if (indiceEscena >= 0 && indiceEscena < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(indiceEscena);
            }
            else
            {
                Debug.LogWarning("Índice de escena no válido o no asignado.");
            }
        }
    }
}
