using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    #region FIELD

    private PlayerAnimation playerAnimation;
    private PlayerController playerController;
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Basic Attack")]
    public float basicAttack01Distance = 0.5f;
    public float basicAttack02Distance = 0.5f;
    public float basicAttack03Distance = 1f;

    private bool atkButtonClickedOnAtk01;
    private bool atkButtonClickedOnAtk02;
    private bool atkButtonClickedOnAtk03;

    private const string attack01 = "Attack01";
    private const string attack02 = "Attack02";
    private const string attack03 = "Attack03";
    private const string notAttacking = "NotAttacking";

    #endregion

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerAnimation = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.isDashing || playerController.isWallJumping || playerController.isWallSliding)
            return;
        AttackCombo();
    }

    void FixedUpdate()
    {
        if (playerController.isDashing || playerController.isWallJumping || playerController.isWallSliding)
            return;
        AttackMove();
    }

    private void AttackCombo()
    {
        // Check if the attack button is pressed and the player is grounded
        bool isAttackButtonPressed = Input.GetMouseButtonDown(0);

        // Handle combo attacks
        string[] attackNames = { "Attack01", "Attack02", "Attack03" };
        string[] triggers = { attack01, attack02, attack03 };

        // Get the current state info
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = currentState.normalizedTime;

        for (int i = 0; i < 3; i++)
        {
            if (currentState.IsName(attackNames[i]))
            {
                // If the attack button is clicked, progress to the next attack
                if (isAttackButtonPressed)
                {
                    if (normalizedTime >= 0.8f) // Start transition after 80% of animation
                    {
                        // Trigger next attack in the combo or stop if no more attacks are chained
                        if (i < 2) // Check if there's a next attack in the combo
                        {
                            animator.SetTrigger(triggers[i + 1]);
                        }
                        else // If it's the last attack in the combo
                        {
                            animator.SetTrigger(notAttacking);
                        }
                    }
                }
                else if (normalizedTime >= 1f) // If the animation is finished and no button is pressed
                {
                    animator.SetTrigger(notAttacking);
                }
                return; // Exit after processing the current attack to avoid unnecessary checks
            }
        }

        // If no combo attack is active and the attack button is pressed, start the first attack
        if (!currentState.IsName("Attack01") && isAttackButtonPressed)
        {
            animator.SetTrigger(attack01);
        }
    }

    private void AttackMove()
    {
        //Move player if player is in attacking state
        float direction = transform.localScale.x > 0 ? 1 : -1;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01"))
            rb.linearVelocity = new Vector2(direction * basicAttack01Distance, rb.linearVelocity.y);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
            rb.linearVelocity = new Vector2(direction * basicAttack02Distance, rb.linearVelocity.y);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
            rb.linearVelocity = new Vector2(direction * basicAttack03Distance, rb.linearVelocity.y);
    }
}
