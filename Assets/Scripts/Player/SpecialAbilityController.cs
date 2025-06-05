using UnityEngine;
using System.Collections;

public class SpecialAbilityController : MonoBehaviour
{
    public int enemigosNecesarios = 5;
    public KeyCode teclaHabilidad = KeyCode.F;

    [Header("Jugador que dispara los proyectiles especiales")]
    public GameObject player;

    [Header("Prefab del proyectil especial")]
    public GameObject prefabProyectilEspecial;

    [Header("Tiempo antes de eliminar enemigos tras disparo")]
    public float delayDesactivacion = 0.5f;

    [Header("Objetos UI para activar al eliminar enemigos (máximo 5)")]
    public GameObject[] objetosUI;

    [Header("Panel que indica que la habilidad está disponible")]
    public GameObject panelHabilidadDisponible;

    private int contadorEnemigosEliminados = 0;
    private bool habilidadDisponible = false;

    void Start()
    {
       
        foreach (GameObject objUI in objetosUI)
        {
            if (objUI != null)
                objUI.SetActive(false);
        }

        if (panelHabilidadDisponible != null)
            panelHabilidadDisponible.SetActive(false);
    }

    void Update()
    {
        if (habilidadDisponible && Input.GetKeyDown(teclaHabilidad))
        {
            ActivarHabilidadEspecial();
        }
    }

    public void RegistrarEnemigoEliminado(GameObject enemigo)
    {
        contadorEnemigosEliminados++;

        // Activar el objeto UI correspondiente si existe y está dentro del rango
        if (contadorEnemigosEliminados <= objetosUI.Length && objetosUI[contadorEnemigosEliminados - 1] != null)
        {
            objetosUI[contadorEnemigosEliminados - 1].SetActive(true);
        }


        if (contadorEnemigosEliminados >= enemigosNecesarios)
        {
            habilidadDisponible = true;

            if (panelHabilidadDisponible != null)

                panelHabilidadDisponible.SetActive(true);
        }
    }

    private void ActivarHabilidadEspecial()
    {
        if (Camera.main == null || player == null || prefabProyectilEspecial == null)
            return;

        Camera cam = Camera.main;
        GameObject[] todos = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in todos)
        {
            if (!obj.activeInHierarchy) continue;
            if (obj.layer != LayerMask.NameToLayer("Enemy")) continue;

            Vector3 viewportPos = cam.WorldToViewportPoint(obj.transform.position);
            bool estaEnCamara = viewportPos.z > 0 && viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

            if (estaEnCamara)
            {
                Vector3 direccion = (obj.transform.position - player.transform.position).normalized;

                GameObject proyectil = Instantiate(prefabProyectilEspecial, player.transform.position, Quaternion.identity);
                Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = direccion * 10f;

                StartCoroutine(DesactivarEnemigoDespues(obj, delayDesactivacion));
            }
        }

        // Reset contador y desactivar UI cuando se usa la habilidad
        contadorEnemigosEliminados = 0;
        habilidadDisponible = false;

        foreach (GameObject objUI in objetosUI)
        {
            if (objUI != null)
                objUI.SetActive(false);
        }

        if (panelHabilidadDisponible != null)
            panelHabilidadDisponible.SetActive(false);
    }

    private IEnumerator DesactivarEnemigoDespues(GameObject enemigo, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemigo != null)
            enemigo.SetActive(false);
    }

}
