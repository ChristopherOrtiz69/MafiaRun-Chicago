using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int hitsParaDesactivar = 3;
    private int golpesRecibidos = 0;
    private SpecialAbilityController habilidadEspecial;


    [Header("Sprite Renderer para efecto de daño")]
    public SpriteRenderer spriteRenderer;
    private Color colorOriginal;
    public float duracionRojo = 0.2f;

    [Header("Efecto de muerte")]
    public int indexVFXMuerte = 0;

    [Header("Audio de muerte")]
    public AudioSource audioSource;
    public AudioClip sonidoMuerte;

    [Header("Objeto extra que se desactiva al morir (opcional)")]
    public GameObject objetoADesactivar;

    [Header("Arma del enemigo (opcional)")]
    public GameObject armaEnemigo;

    [Header("Script de disparos del enemigo (opcional)")]
    public MonoBehaviour scriptDisparo;

    private Collider2D miCollider;

    // Eventos públicos para conectar con otros scripts
    public delegate void EventoGolpe();
    public event EventoGolpe OnRecibirGolpe;
    public event EventoGolpe OnMorir;

    void Start()
    {
        if (spriteRenderer != null)
            colorOriginal = spriteRenderer.color;
        else
            Debug.LogWarning("No asignaste el SpriteRenderer en EnemyHealth.");

        miCollider = GetComponent<Collider2D>();
        if (miCollider == null)
            Debug.LogWarning("EnemyHealth no encontró un Collider2D en el mismo GameObject.");

        habilidadEspecial = FindObjectOfType<SpecialAbilityController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bala = collision.GetComponent<Bullet>();
        if (bala != null && !bala.esDelEnemigo)
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

        if (habilidadEspecial != null)
            habilidadEspecial.RegistrarEnemigoEliminado(gameObject);


        // Disparar evento de daño
        OnRecibirGolpe?.Invoke();

        if (golpesRecibidos >= hitsParaDesactivar)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            if (miCollider != null)
                miCollider.enabled = false;

            if (objetoADesactivar != null)
                objetoADesactivar.SetActive(false);

            if (armaEnemigo != null)
                armaEnemigo.SetActive(false);

            // Desactiva el script de disparo
            if (scriptDisparo != null)
            {
                scriptDisparo.CancelInvoke();
                scriptDisparo.StopAllCoroutines();
                scriptDisparo.enabled = false;
            }

            // Disparar evento de muerte
            OnMorir?.Invoke();

            EjecutarVFX();
            ReproducirSonidoMuerte();

            float tiempoEsperar = (sonidoMuerte != null) ? sonidoMuerte.length + 0.1f : 0.5f;
            StartCoroutine(DesactivarDespuesDe(tiempoEsperar));
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
