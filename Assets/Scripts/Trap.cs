using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float damage;
    private Animator animator;
    private bool isTrapClosed = false;  // Flag to track if the trap is closed

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator of the Trap
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only apply damage if the trap is open
        if (!isTrapClosed && collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            animator.SetTrigger("Trapclose"); // Trigger trap close animation
            Script player = collision.GetComponent<Script>();
            if (player != null)
            {
                player.isTrap(); // Lock player's movement (trap triggered)
            }
            // Close the trap to prevent further damage
            CloseTrap();
        }
    }

    // This method will be called to close the trap and stop dealing damage
    private void CloseTrap()
    {
        isTrapClosed = true;
    }

    // This method will be called to open the trap and allow damage again
    public void OpenTrap()
    {
        isTrapClosed = false;
    }
}
