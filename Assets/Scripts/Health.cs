using EthanTheHero;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private Animator animator;
    private bool dead;
    public float currentHealth { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = startingHealth;
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            animator.SetTrigger("PlayerHurt");
        }
        else
        {
            if (!dead)
            {
                animator.SetTrigger("PlayerDeath");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
            }

        }
    }

}
