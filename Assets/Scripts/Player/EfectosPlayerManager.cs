using UnityEngine;
using Cinemachine;
using System.Collections;

public class EfectosPlayerManager : MonoBehaviour
{
    [Header("Referencias necesarias")]
    public Health health; // Asegúrate de usar el tipo correcto
    public SpriteRenderer spriteRenderer;
    public CinemachineImpulseSource impulseSource;

    [Header("Configuración del efecto")]
    public float duracionColor = 0.2f;
    public Color colorDaño = Color.red;

    private Color colorOriginal;

    private void Start()
    {
        if (health != null)
        {
            health.OnRecibirGolpeEvent += EjecutarEfectosDaño;
        }

        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnRecibirGolpeEvent -= EjecutarEfectosDaño;
        }
    }

    void EjecutarEfectosDaño()
    {
        StartCoroutine(ParpadeoColor());

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }

    IEnumerator ParpadeoColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorDaño;
            yield return new WaitForSeconds(duracionColor);
            spriteRenderer.color = colorOriginal;
        }
    }
}
