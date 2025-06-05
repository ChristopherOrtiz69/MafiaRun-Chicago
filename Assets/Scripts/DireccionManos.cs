using UnityEngine;

public class direccionManos : MonoBehaviour
{
    public Transform playerTransform;

    private Animator animator;
    private Vector3 escalaOriginal;
    private Vector2 ultimaDireccion = Vector2.right;

    void Start()
    {
        animator = GetComponent<Animator>();
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
       
        transform.position = playerTransform.position;
        transform.rotation = Quaternion.identity;

        // Capturar entrada para actualizar dirección
        if (Input.GetKey(KeyCode.W)) ultimaDireccion = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) ultimaDireccion = Vector2.down;
        else if (Input.GetKey(KeyCode.D)) ultimaDireccion = Vector2.right;
        else if (Input.GetKey(KeyCode.A)) ultimaDireccion = Vector2.left;

       
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        // Elegir animación y escala
        if (Mathf.Abs(ultimaDireccion.x) > Mathf.Abs(ultimaDireccion.y))
        {
           
            if (ultimaDireccion.x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
        }
        else
        {
            
            if (ultimaDireccion.y > 0)
            {
                animator.SetBool("Up", true);
                transform.localScale = escalaOriginal;
            }
            else
            {
                animator.SetBool("Down", true);
                transform.localScale = escalaOriginal;
            }
        }
    }
}
