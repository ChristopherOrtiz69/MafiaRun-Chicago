using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private EnemyHealth enemyHealth;
    private Rigidbody2D rb;

    [Header("Opcional: Movimiento")]
    public EnemyAI enemyAI;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("AnimatorController: No se encontró Animator en este objeto.");
        }

        rb = GetComponentInParent<Rigidbody2D>();
        enemyHealth = GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.OnRecibirGolpe += ActivarAnimacionGolpe;
            enemyHealth.OnMorir += ActivarAnimacionMuerte;
        }
        else
        {
            Debug.LogWarning("AnimatorController: No se encontró EnemyHealth en el padre.");
        }
    }

    void Update()
    {
        if (animator == null || enemyAI == null || rb == null)
            return;

        float velocidadX = Mathf.Abs(rb.velocity.x);
        bool estaCaminando = velocidadX > 0.1f;
        animator.SetBool("Caminar", estaCaminando);
    }

    void ActivarAnimacionGolpe()
    {
        if (animator != null)
            animator.SetTrigger("Golpeado");
    }

    void ActivarAnimacionMuerte()
    {
        if (animator != null)
            animator.SetTrigger("Morir");
    }
}
