using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public event Action OnRecibirGolpe;
    public event Action OnMorir;

    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    void Awake()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirGolpe(int danio)
    {
        vidaActual -= danio;
        OnRecibirGolpe?.Invoke();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void Morir()
    {
        OnMorir?.Invoke();
        Destroy(gameObject);
    }
}
