using UnityEngine;

public class DefaultBulletFactory : IBulletFactory
{
    public GameObject CrearBala(GameObject prefab)
    {
        GameObject bala = Object.Instantiate(prefab);
        bala.SetActive(false);
        return bala;
    }
}
