using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public int poolSizePorPrefab = 10;

    // Diccionario para almacenar múltiples tipos de balas
    private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();

    void Awake()
    {
        Instance = this;
    }

    public GameObject ObtenerBala(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
        {
            // Si no existe el pool para este prefab, lo creamos
            pools[prefab] = new List<GameObject>();

            for (int i = 0; i < poolSizePorPrefab; i++)
            {
                GameObject nuevaBala = Instantiate(prefab);
                nuevaBala.SetActive(false);
                pools[prefab].Add(nuevaBala);
            }
        }

        // Buscar una bala inactiva
        foreach (GameObject bala in pools[prefab])
        {
            if (!bala.activeInHierarchy)
            {
                return bala;
            }
        }

        // Si no hay disponibles, crear una nueva
        GameObject extraBala = Instantiate(prefab);
        extraBala.SetActive(false);
        pools[prefab].Add(extraBala);
        return extraBala;
    }
}
