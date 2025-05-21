using UnityEngine;
using Cinemachine;

public class CameraShakeOnShoot : MonoBehaviour
{
    [Header("Referencia al Impulse Source")]
    public CinemachineImpulseSource impulseSource;

    [Header("Tecla de prueba (opcional)")]
    public KeyCode triggerKey = KeyCode.Mouse0;

    void Update()
    {
        // Disparo de prueba con tecla
        if (Input.GetKeyDown(triggerKey))
        {
            ShakeCamera();
        }
    }

    // Método público para llamar desde otro script si prefieres
    public void ShakeCamera()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
        else
        {
            Debug.LogWarning("No hay un CinemachineImpulseSource asignado.");
        }
    }
}
