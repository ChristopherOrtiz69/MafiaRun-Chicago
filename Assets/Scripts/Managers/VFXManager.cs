using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }
    [SerializeField] private GameObject[] listaVFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ActivarVFX(int index, Vector3 posicion)
    {
        if (!EsIndexValido(index)) return;

        GameObject vfx = Instantiate(listaVFX[index], posicion, Quaternion.identity);
        Destroy(vfx, 1f);
    }

    private bool EsIndexValido(int index)
    {
        if (listaVFX == null || index < 0 || index >= listaVFX.Length || listaVFX[index] == null)
        {
            Debug.LogWarning($"VFXManager: Índice {index} inválido o sin asignar.");
            return false;
        }
        return true;
    }
}
