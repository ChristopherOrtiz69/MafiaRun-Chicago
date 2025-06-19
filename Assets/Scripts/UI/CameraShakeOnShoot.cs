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
       
        if (Input.GetKeyDown(triggerKey))
        {
            ShakeCamera();
        }
    }

    
    public void ShakeCamera()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
      
    }
}
