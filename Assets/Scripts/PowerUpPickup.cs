using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public Weapon nuevaArma;
    public Disparo arma; // referencia que arrastras en inspector

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
            Destroy(gameObject);
        }
    }
}
