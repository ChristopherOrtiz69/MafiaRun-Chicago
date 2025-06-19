using UnityEngine;
using System;

public class VidaJugador : MonoBehaviour
{
    [Header("Configuración de vidas")]
    [SerializeField] private int maxImpactos = 3;
    private int impactosRecibidos = 0;

    public event Action<int> OnVidaCambiada; 
    public event Action OnVidasAgotadas;

    public void RegistrarImpacto()
    {
        impactosRecibidos++;

        int vidasRestantes = maxImpactos - impactosRecibidos;
        OnVidaCambiada?.Invoke(vidasRestantes);

        if (impactosRecibidos >= maxImpactos)
        {
            OnVidasAgotadas?.Invoke();
        }
    }

    public int GetImpactos() => impactosRecibidos;
    public int GetMaxImpactos() => maxImpactos;
}
