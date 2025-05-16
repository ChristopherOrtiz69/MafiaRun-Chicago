using UnityEngine;

public class direccionManos : MonoBehaviour
{
    public Transform playerTransform; 

    private Animator animator;
    private Vector3 escalaOriginal;

    void Start()
    {
        animator = GetComponent<Animator>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        // Mantener la posición del player, sin heredar rotación ni escala
        transform.position = playerTransform.position;

        // Resetear rotación siempre
        transform.rotation = Quaternion.identity;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector2 direccion = (mousePos - transform.position).normalized;

        // Limpiar todos los bools
       // animator.SetBool("Right", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            // Horizontal
           // animator.SetBool("Right", true);

            if (direccion.x > 0)
            {
                // Mirando derecha normal
                transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
            else
            {
                // Mirando izquierda: flip horizontal con misma animación
                transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
        }
        else
        {
            // Vertical
            if (direccion.y > 0)
            {
                animator.SetBool("Up", true);
                transform.localScale = escalaOriginal; // sin flip para arriba
            }
            else
            {
                animator.SetBool("Down", true);
                transform.localScale = escalaOriginal; // sin flip para abajo
            }
        }
    }
}
