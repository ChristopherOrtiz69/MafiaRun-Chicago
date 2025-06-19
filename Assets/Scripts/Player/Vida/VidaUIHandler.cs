using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VidaUIHandler : MonoBehaviour, IVidaUI
{
    [Tooltip("Lista de íconos de vida en orden (de izquierda a derecha)")]
    [SerializeField] private List<GameObject> iconosVida;

    [SerializeField] private VidaJugador vidaJugador;

    private void Awake()
    {
        if (vidaJugador == null)
        {
            Debug.LogError("VidaJugador no asignado en VidaUIHandler");
            return;
        }

        vidaJugador.OnVidaCambiada += ActualizarVidas;
    }

    public void ActualizarVidas(int vidasRestantes)
    {
        for (int i = 0; i < iconosVida.Count; i++)
        {
            Image imagen = iconosVida[i].GetComponent<Image>();
            if (imagen != null)
            {
                imagen.color = (i < vidasRestantes) ? Color.white : new Color(1, 1, 1, 0);
            }
        }
    }

    private void OnDestroy()
    {
        if (vidaJugador != null)
        {
            vidaJugador.OnVidaCambiada -= ActualizarVidas;
        }
    }
}
