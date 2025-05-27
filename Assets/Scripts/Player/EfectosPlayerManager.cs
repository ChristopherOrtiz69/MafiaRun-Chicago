using UnityEngine;
using Cinemachine;
using System.Collections;

public class EfectosPlayerManager : MonoBehaviour
{
    [Header("Referencias necesarias")]
    public VidaJugador vidaJugador; // Arrastra aquí el Player que tiene el script VidaJugador
    public SpriteRenderer spriteRenderer; // Sprite del jugador
    public CinemachineImpulseSource impulseSource;

    [Header("Configuración del efecto")]
    public float duracionColor = 0.2f;
    public Color colorDaño = Color.red;

    private Color colorOriginal;

    private void Start()
    {
        if (vidaJugador != null)
        {
            vidaJugador.OnRecibirDaño += EjecutarEfectosDaño;
        }

        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    private void OnDestroy()
    {
        // Siempre desvincula el evento para evitar errores
        if (vidaJugador != null)
        {
            vidaJugador.OnRecibirDaño -= EjecutarEfectosDaño;
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
