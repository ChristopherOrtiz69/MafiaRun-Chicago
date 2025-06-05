using UnityEngine;

public class ActivarObjetoAlDesaparecer : MonoBehaviour
{
    [Tooltip("Objeto que se activará cuando este desaparezca")]
    public GameObject objetoAActivar;

    private void OnDisable()
    {
        Activar();
    }

    private void OnDestroy()
    {
        Activar();
    }

    private void Activar()
    {
        if (objetoAActivar != null)
        {
            objetoAActivar.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se asignó ningún objeto para activar.");
        }
    }
}
