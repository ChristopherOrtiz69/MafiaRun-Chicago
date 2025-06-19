using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Spawneo")]
    public GameObject prefab;
    public float intervaloSpawn = 3f;
    public int cantidadEnemigos = 10;

    [Header("Asignación")]
    public GameObject player;

    [Header("Panel al finalizar (opcional)")]
    public GameObject panelFinal;

    private int enemigosSpawneados = 0;

    private void Start()
    {
        if (prefab == null || player == null)
        {
            Debug.LogWarning("Falta asignar prefab o player en el inspector.");
            return;
        }

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (enemigosSpawneados < cantidadEnemigos)
        {
            GameObject enemigo = Instantiate(prefab, transform.position, transform.rotation);

            EnemyAI enemyAI = enemigo.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.SetObjetivo(player.transform);  // <-- Aquí usamos el método público para asignar objetivo
            }
            else
            {
                Debug.LogWarning("El prefab no tiene EnemyAI.");
            }

            enemigosSpawneados++;
            yield return new WaitForSeconds(intervaloSpawn);
        }

        TerminoSpawneo();
    }

    public void TerminoSpawneo()
    {
        if (panelFinal != null)
        {
            panelFinal.SetActive(true);
        }
    }
}
