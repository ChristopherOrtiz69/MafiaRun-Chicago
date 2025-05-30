using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacionesUI : MonoBehaviour
{
    [Header("Cartas")]
    public GameObject carta;
    public GameObject carta1;
    public GameObject carta2;
    public GameObject carta3;
    public GameObject carta4;

    [Header("Botones")]
    public GameObject boton;
    public GameObject boton1;
    public GameObject boton2;

    [Header("Texto")]
    public GameObject TextMenu;

    [Header("Panel a mover")]
    public GameObject panel;

    [Header("Canvas a desvanecer")]
    public CanvasGroup canvasGroupADesvanecer;

    [Header("Imagen con animación previa")]
    public GameObject imagenAnimada;

    private void Start()
    {
        if (imagenAnimada != null)
        {
            RectTransform rectTransform = imagenAnimada.GetComponent<RectTransform>();

            // Movimiento Y hacia 0 con easeOutBounce
            LeanTween.moveY(rectTransform, 0f, 2.5f)
                .setEase(LeanTweenType.easeOutBounce)
                .setOnComplete(DesvanecerCanvasGroup);
        }
        else
        {
            Debug.LogWarning("Imagen animada no asignada. Saltando animación.");
            DesvanecerCanvasGroup();
        }
    }

    void DesvanecerCanvasGroup()
    {
        if (canvasGroupADesvanecer != null)
        {
            LeanTween.alphaCanvas(canvasGroupADesvanecer, 0f, 1f)
                .setOnComplete(() =>
                {
                    canvasGroupADesvanecer.blocksRaycasts = false;
                    canvasGroupADesvanecer.interactable = false;
                    IniciarAnimacionesUI();
                });
        }
        else
        {
            Debug.LogWarning("CanvasGroup no asignado. Se inician animaciones directamente.");
            IniciarAnimacionesUI();
        }
    }

    void IniciarAnimacionesUI()
    {
        LeanTween.moveX(carta.GetComponent<RectTransform>(), -697, 1.4f).setDelay(0.5f).setEase(LeanTweenType.easeInBounce);
        LeanTween.moveX(carta1.GetComponent<RectTransform>(), -621, 1.2f).setDelay(0.5f).setEase(LeanTweenType.easeInBounce);
        LeanTween.moveX(carta2.GetComponent<RectTransform>(), -545, 1.0f).setDelay(0.5f).setEase(LeanTweenType.easeInBounce);
        LeanTween.moveX(carta3.GetComponent<RectTransform>(), -469, 0.9f).setDelay(0.5f).setEase(LeanTweenType.easeInBounce);
        LeanTween.moveX(carta4.GetComponent<RectTransform>(), -394, 0.8f).setDelay(0.5f).setEase(LeanTweenType.easeInBounce);

        LeanTween.moveY(boton.GetComponent<RectTransform>(), -27, 0.7f).setDelay(0.1f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveY(boton1.GetComponent<RectTransform>(), -130, 0.7f).setDelay(0.1f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveY(boton2.GetComponent<RectTransform>(), -233, 0.7f).setDelay(0.1f).setEase(LeanTweenType.easeOutBounce);
        LeanTween.moveY(TextMenu.GetComponent<RectTransform>(), 7, 0.5f).setDelay(0.2f).setEase(LeanTweenType.easeOutElastic);
    }

    public void MoverPanelAX()
    {
        if (panel != null)
        {
            LeanTween.moveX(panel.GetComponent<RectTransform>(), 0f, 0.8f).setEase(LeanTweenType.easeOutQuart);
        }
    }

    public void MoverPanelRegreso()
    {
        if (panel != null)
        {
            LeanTween.moveX(panel.GetComponent<RectTransform>(), -1830f, 0.8f).setEase(LeanTweenType.easeInBack);
        }
    }
}
