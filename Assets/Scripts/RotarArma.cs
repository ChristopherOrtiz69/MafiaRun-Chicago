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

        Vector2 direccion = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) direccion = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) direccion = Vector2.down;
        else if (Input.GetKey(KeyCode.D)) direccion = Vector2.right;
        else if (Input.GetKey(KeyCode.A)) direccion = Vector2.left;
        else return; // No hay dirección, no hacer nada

        Transform pivotObjetivo = pivotRight;
        float angulo = 0f;

        // Asignar posición según pivote
        if (direccion == Vector2.right)
        {
            pivotObjetivo = pivotRight;
            angulo = 0f;
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
        }
        else if (direccion == Vector2.left)
        {
            pivotObjetivo = pivotLeft;
            angulo = 0f; // Sin rotación para evitar que gire boca abaje
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
