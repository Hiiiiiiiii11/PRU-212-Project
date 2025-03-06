using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float damage;
    private Animator animator;
    private bool isTrapClosed = false;
    private Audio audio;
    private Coroutine damageCoroutine; // Coroutine để trừ máu mỗi 3s

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audio = FindAnyObjectByType<Audio>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTrapClosed && collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            audio.PlayTrapClose();
            audio.PlayPlayerHurt();
            animator.SetTrigger("Trapclose"); // Trigger trap close animation
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.isTrap(); // Lock player's movement (trap triggered)
            }
            // Close the trap to prevent further damage
            CloseTrap();
            damageCoroutine = StartCoroutine(DamageOverTime(player));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine); // Dừng trừ máu khi rời khỏi bẫy
            damageCoroutine = null;
        }
    }

    private IEnumerator DamageOverTime(Player player)
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            player.GetComponent<Health>().TakeDamage(1f);
            audio.PlayPlayerHurt();
        }
    }

    private void CloseTrap()
    {
        isTrapClosed = true;
    }

    public void OpenTrap()
    {
        isTrapClosed = false;
    }
}
