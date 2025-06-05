using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    public GameObject[] listaVFX; // Asigna los VFX desde el Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ActivarVFX(int index, Vector3 posicion)
    {
        if (index >= 0 && index < listaVFX.Length && listaVFX[index] != null)
        {
            GameObject vfx = Instantiate(listaVFX[index], posicion, Quaternion.identity);

            // Opción 1: Destruir el VFX después de cierto tiempo (ej. 1 segundo)
            Destroy(vfx, 1f);
        }
        else
        {
            Debug.LogWarning("Index de VFX inválido o no asignado.");
        }
    }
}
