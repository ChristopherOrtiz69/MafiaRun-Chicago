using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int hitsParaDesactivar = 3;
    private int golpesRecibidos = 0;

    [Header("Sprite Renderer para efecto de daño")]
    public SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    public float duracionRojo = 0.2f;

    [Header("Efecto de muerte")]
    public int indexVFXMuerte = 0;

    [Header("Audio de muerte")]
    public AudioSource audioSource;
    public AudioClip sonidoMuerte;

    void Start()
    {
        if (spriteRenderer != null)
            colorOriginal = spriteRenderer.color;
        else
            Debug.LogWarning("No asignaste el SpriteRenderer en EnemyHealth.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bala = collision.GetComponent<Bullet>();
        if (bala != null && bala.esDelEnemigo == false)
        {
            RecibirGolpe();
            bala.gameObject.SetActive(false);
        }
    }

    void RecibirGolpe()
    {
        golpesRecibidos++;
        if (spriteRenderer != null)
            StartCoroutine(PintarRojoTemporal());

        if (golpesRecibidos >= hitsParaDesactivar)
        {
            EjecutarVFX();
            ReproducirSonidoMuerte();
            StartCoroutine(DesactivarDespuesDe(0.15f)); // Solo espera para que se vea el VFX
        }
    }

    IEnumerator PintarRojoTemporal()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duracionRojo);
        spriteRenderer.color = colorOriginal;
    }

    void EjecutarVFX()
    {
        if (VFXManager.Instance != null)
        {
            VFXManager.Instance.ActivarVFX(indexVFXMuerte, transform.position);
        }
    }

    void ReproducirSonidoMuerte()
    {
        if (audioSource != null && sonidoMuerte != null)
        {
            audioSource.PlayOneShot(sonidoMuerte);
        }
    }

    IEnumerator DesactivarDespuesDe(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        gameObject.SetActive(false);
    }
}
