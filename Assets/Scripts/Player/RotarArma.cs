using UnityEngine;

public class RotarArma : MonoBehaviour
{
    [Header("Pivotes de posicionamiento")]
    public Transform pivotRight;
    public Transform pivotLeft;
    public Transform pivotUp;
    public Transform pivotDown;

    [Header("Referencia de entrada")]
    public MonoBehaviour fuenteEntrada; 

    private IDireccionInput direccionInput;
    private Vector3 escalaOriginal;

    void Awake()
    {
        escalaOriginal = transform.localScale;

        // Casteo seguro a la interfaz
        direccionInput = fuenteEntrada as IDireccionInput;
        if (direccionInput == null)
        {
            Debug.LogError("La fuente de entrada no implementa IDireccionInput.");
        }
    }

    void LateUpdate()
    {
        if (direccionInput == null) return;

        Vector2 direccion = direccionInput.ObtenerDireccion();
        if (direccion == Vector2.zero) return;

        Transform pivotObjetivo = pivotRight;
        float angulo = 0f;

        if (direccion == Vector2.right)
        {
            pivotObjetivo = pivotRight;
            angulo = 0f;
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (direccion == Vector2.left)
        {
            pivotObjetivo = pivotLeft;
            angulo = 0f;
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (direccion == Vector2.up)
        {
            pivotObjetivo = pivotUp;
            angulo = 90f;
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (direccion == Vector2.down)
        {
            pivotObjetivo = pivotDown;
            angulo = -90f;
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }

        transform.position = pivotObjetivo.position;
        transform.rotation = Quaternion.Euler(0, 0, angulo);
    }
}
