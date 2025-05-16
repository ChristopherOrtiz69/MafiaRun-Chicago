using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject balaPrefab;
    public int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bala = Instantiate(balaPrefab);
            bala.SetActive(false);
            pool.Add(bala);
        }
    }

    public GameObject ObtenerBala()
    {
        foreach (GameObject bala in pool)
        {
            if (!bala.activeInHierarchy)
            {
                return bala;
            }
        }

        // Si no hay balas disponibles, se instancia otra
        GameObject nuevaBala = Instantiate(balaPrefab);
        nuevaBala.SetActive(false);
        pool.Add(nuevaBala);
        return nuevaBala;
    }
}
