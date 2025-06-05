using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusManager : MonoBehaviour
{
  
    public void RegresarAEscena(int index)
    {
        if (index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(index);
        }
        else
        {
            Debug.LogWarning("Índice de escena fuera de rango: " + index);
        }
    }

   
    public void ReiniciarEscenaActual()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual);
    }
}
