using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public Weapon nuevaArma;
    public Disparo arma;

    [Header("Sonido al recoger")]
    public AudioClip sonidoRecoger;
    public AudioSource audioSourcePrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (arma != null)
            {
                arma.CambiarArma(nuevaArma);
                Debug.Log("PowerUp recogido, arma cambiada.");
            }
            else
            {
                Debug.LogWarning("No hay referencia al arma para cambiar.");
            }

            if (sonidoRecoger != null && audioSourcePrefab != null)
            {
                // Crear objeto temporal con audio
                AudioSource fuente = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
                fuente.clip = sonidoRecoger;
                fuente.Play();
                Destroy(fuente.gameObject, sonidoRecoger.length);
            }

         
            gameObject.SetActive(false);


            Destroy(gameObject, 0.05f);
        }
    }
}