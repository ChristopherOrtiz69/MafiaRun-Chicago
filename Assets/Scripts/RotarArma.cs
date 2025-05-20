using UnityEngine;

public class RotarArma : MonoBehaviour
{
    [Header("Pivotes de posicionamiento")]
    public Transform pivotRight;
    public Transform pivotLeft;
    public Transform pivotUp;
    public Transform pivotDown;

    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;

        if (pivotRight == null || pivotLeft == null || pivotUp == null || pivotDown == null)
        {
            Debug.LogWarning("Pivotes no asignados. Asegúrate de pasarlos desde Disparo.cs.");
        }
    }

    void LateUpdate()
    {
        if (pivotRight == null || pivotLeft == null || pivotUp == null || pivotDown == null)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direccion = (mousePos - pivotRight.position).normalized;

        Transform pivotObjetivo = pivotRight;
        float angulo = 0f;

        transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            if (direccion.x > 0)
            {
                pivotObjetivo = pivotRight;
                angulo = 0f;
            }
            else
            {
                pivotObjetivo = pivotLeft;
                angulo = 0f;
                transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
        }
        else
        {
            if (direccion.y > 0)
            {
                pivotObjetivo = pivotUp;
                angulo = 90f;
            }
            else
            {
                pivotObjetivo = pivotDown;
                angulo = -90f;
            }
        }

        transform.position = pivotObjetivo.position;
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }
}
