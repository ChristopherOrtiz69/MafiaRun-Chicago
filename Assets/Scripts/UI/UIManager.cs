using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Cambia a una escena usando su índice en Build Settings
    public void CambiarEscenaPorIndice(int indiceEscena)
    {
        if (indiceEscena >= 0 && indiceEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(indiceEscena);
        }
        else
        {
            Debug.LogWarning("Índice de escena fuera de rango: " + indiceEscena);
        }
    }

    // Aquí podrías agregar más funciones como mostrar/ocultar paneles, salir del juego, etc.
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
