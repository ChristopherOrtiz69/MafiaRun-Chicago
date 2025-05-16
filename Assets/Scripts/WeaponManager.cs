using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform armaHolder;
    private GameObject armaActual;

    public void CambiarArma(Weapon nuevaArma)
    {
        if (armaActual != null)
            Destroy(armaActual);

        if (nuevaArma.prefabVisualArma != null && armaHolder != null)
        {
            armaActual = Instantiate(nuevaArma.prefabVisualArma, armaHolder.position, armaHolder.rotation, armaHolder);
        }
        else
        {
            Debug.LogWarning("Falta asignar prefabVisualArma o armaHolder");
        }
    }
}
