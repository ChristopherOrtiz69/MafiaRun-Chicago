using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private Health health;

    void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        if (health != null)
        {
            health.OnRecibirGolpeEvent += () => animator.SetTrigger("Golpeado");
            health.OnMorirEvent += () => animator.SetTrigger("Morir");
        }
    }
}

