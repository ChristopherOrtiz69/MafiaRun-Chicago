using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private float fireRate = 1f;

    private float proximoDisparo;

    private Vector2 objetivoCentro;
    private Vector2 direccion;

    public void IntentarDisparar(Transform objetivo)
    {
        if (Time.time < proximoDisparo) return;
        if (puntoDisparo == null || prefabBala == null || objetivo == null || BulletPool.Instance == null) return;

        GameObject bala = BulletPool.Instance.ObtenerBala(prefabBala);
        if (bala == null) return;

        bala.transform.SetPositionAndRotation(puntoDisparo.position, Quaternion.identity);
        bala.SetActive(true);

        Collider2D colliderJugador = objetivo.GetComponent<Collider2D>();
        objetivoCentro = colliderJugador != null ? colliderJugador.bounds.center : (Vector2)objetivo.position;

        direccion = (objetivoCentro - (Vector2)puntoDisparo.position).normalized;

        Bullet bulletScript = bala.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.esDelEnemigo = true;
            bulletScript.Disparar(direccion);
        }

        proximoDisparo = Time.time + fireRate;
    }
}
