using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public int poolSizePorPrefab = 20;

    private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();
    private IBulletFactory bulletFactory;

    private void Awake()
    {
        Instance = this;
        bulletFactory = new DefaultBulletFactory();
    }

    public void InicializarPoolPara(GameObject prefab)
    {
        if (pools.ContainsKey(prefab)) return;

        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < poolSizePorPrefab; i++)
        {
            GameObject bala = bulletFactory.CrearBala(prefab);
            bala.SetActive(false);
            pool.Add(bala);
        }
        pools[prefab] = pool;
    }

    public GameObject ObtenerBala(GameObject prefab)
    {
        if (!pools.ContainsKey(prefab))
            InicializarPoolPara(prefab);

        foreach (GameObject bala in pools[prefab])
        {
            if (!bala.activeInHierarchy)
            {
                return bala;
            }
        }

        // 🔴 Ya no se crean más si se agota el pool (para evitar GC)
        Debug.LogWarning($"[BulletPool] No hay balas disponibles para {prefab.name}. Aumenta el tamaño del pool.");
        return null;
    }

    
    public void DevolverBala(GameObject bala)
    {
        bala.SetActive(false);
    }
}
