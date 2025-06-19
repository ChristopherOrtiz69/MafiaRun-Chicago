using UnityEngine;

public class InicioNivel : MonoBehaviour
{
    [Header("Letras")]
    public RectTransform letra1;
    public RectTransform letra2;
    public RectTransform letra3;
    public RectTransform letra4;
    public RectTransform letra5;
    public RectTransform letra6;
    public RectTransform Panel;

    [Header("Configuración de animación")]
    public LeanTweenType tipoAnimacion = LeanTweenType.easeOutBack;

    private Vector3[] posicionesIniciales;
    private GameObject canvas;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>()?.gameObject;
        GuardarPosicionesIniciales();
        IniciarAnimacionLetras();
    }

    void GuardarPosicionesIniciales()
    {
        posicionesIniciales = new Vector3[7];
        posicionesIniciales[0] = letra1.anchoredPosition;
        posicionesIniciales[1] = letra2.anchoredPosition;
        posicionesIniciales[2] = letra3.anchoredPosition;
        posicionesIniciales[3] = letra4.anchoredPosition;
        posicionesIniciales[4] = letra5.anchoredPosition;
        posicionesIniciales[5] = letra6.anchoredPosition;
        posicionesIniciales[6] = Panel.anchoredPosition;
    }

    void IniciarAnimacionLetras()
    {
        LeanTween.moveX(letra1, -324f, 0.5f).setDelay(0f).setEase(tipoAnimacion);
        LeanTween.moveX(letra2, -192f, 0.5f).setDelay(0.2f).setEase(tipoAnimacion);
        LeanTween.moveX(letra3, -63f, 0.5f).setDelay(0.4f).setEase(tipoAnimacion);
        LeanTween.moveX(letra4, 101f, 0.5f).setDelay(0.6f).setEase(tipoAnimacion);
        LeanTween.moveX(letra5, 253f, 0.5f).setDelay(0.8f).setEase(tipoAnimacion);
        LeanTween.moveX(letra6, 450f, 0.5f).setDelay(1.0f).setEase(tipoAnimacion);
        LeanTween.moveY(Panel, 152f, 0.5f).setDelay(0.1f).setEase(tipoAnimacion)
            .setOnComplete(() =>
            {
                Invoke(nameof(CerrarCanvas), 2f);
            });
    }

    void CerrarCanvas()
    {
        RestaurarPosiciones();
        if (canvas != null)
            canvas.SetActive(false);
    }

    void RestaurarPosiciones()
    {
        LeanTween.move(letra1, posicionesIniciales[0], 0.5f).setEase(tipoAnimacion);
        LeanTween.move(letra2, posicionesIniciales[1], 0.5f).setEase(tipoAnimacion);
        LeanTween.move(letra3, posicionesIniciales[2], 0.5f).setEase(tipoAnimacion);
        LeanTween.move(letra4, posicionesIniciales[3], 0.5f).setEase(tipoAnimacion);
        LeanTween.move(letra5, posicionesIniciales[4], 0.5f).setEase(tipoAnimacion);
        LeanTween.move(letra6, posicionesIniciales[5], 0.5f).setEase(tipoAnimacion);
        LeanTween.move(Panel, posicionesIniciales[6], 0.5f).setEase(tipoAnimacion);
    }
}
