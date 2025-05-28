using UnityEngine;

public class GunSoundManager : MonoBehaviour
{
    [Header("Referencia al AudioSource del arma")]
    public AudioSource audioSource;

    [Header("Sonido del disparo")]
    public AudioClip sonidoDisparo;

    public void ReproducirSonidoDisparo()
    {
        Debug.Log("Intentando reproducir sonido de disparo");

        if (audioSource != null && sonidoDisparo != null)
        {
            audioSource.PlayOneShot(sonidoDisparo);
            Debug.Log("¡Sonido de disparo reproducido!");
        }
        else
        {
            Debug.LogWarning("Falta AudioSource o sonidoDisparo en GunSoundManager");
        }
    }

}
