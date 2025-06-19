using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float vidaMaxima = 100f;
    private float vidaActual;

    public event Action OnMorirEvent;
    public event Action OnRecibirGolpeEvent;

    public int vfxIndexMuerte = 0;

    [Header("Sprite que se iluminará al recibir daño")]
    public SpriteRenderer spriteRenderer;

    public Color colorDaño = Color.red;
    public float duracionParpadeo = 0.15f;

    private Color colorOriginal;
    private float tiempoParpadeoRestante;

    void Start()
    {
        vidaActual = vidaMaxima;
        if (spriteRenderer != null)
            colorOriginal = spriteRenderer.color;
    }

    void Update()
    {
        if (tiempoParpadeoRestante > 0)
        {
            tiempoParpadeoRestante -= Time.deltaTime;
            if (tiempoParpadeoRestante <= 0 && spriteRenderer != null)
                spriteRenderer.color = colorOriginal;
        }
    }

    public void RecibirDaño(float cantidad)
    {
        vidaActual -= cantidad;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorDaño;
            tiempoParpadeoRestante = duracionParpadeo;
        }

        OnRecibirGolpeEvent?.Invoke();

        if (vidaActual <= 0)
        {
            OnMorirEvent?.Invoke();

            if (VFXManager.Instance != null)
                VFXManager.Instance.ActivarVFX(vfxIndexMuerte, transform.position);

            Destroy(gameObject);
        }
    }
}
