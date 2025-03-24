using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace level3
{
    public class PlayerAnimation : MonoBehaviour
    {
        private PlayerController playerController;
        private Animator animator;
        private Rigidbody2D rb;
        private PlayerAttack playerAttack;

        private const string speed = "Speed";
        private const string runIdle = "RunIdlePlayying";
        private const string jump = "Grounded";
        private const string yVelocity = "Yvelocity";
        private const string dash = "Dashing";
        private const string wallSliding = "WallSliding";
        private const string wallJump = "WallJump";
        private const string hurt = "Hurt";
        private const string hurtEnded = "HurtEnded";
        private const string death = "Death";
        private const string deathEnded = "DeathEnded";

        private bool runIdleIsPlayying;
        private bool isHurt = false;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            playerAttack = GetComponent<PlayerAttack>();
        }

        void Update()
        {
            IdleAndRun();
            Jump();
            Dash();
            WallSlidingAndJumping();
        }

        public void IdleAndRun()
        {
            animator.SetFloat(speed, Mathf.Abs(playerController.move.x));
            //Set Run Animation
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunIdleTrans"))
            {
                runIdleIsPlayying = true;
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    runIdleIsPlayying = false;
            }
            animator.SetBool(runIdle, runIdleIsPlayying);
        }

        public void Jump()
        {
            animator.SetBool(jump, playerController.IsGrounded());
            animator.SetFloat(yVelocity, rb.linearVelocity.y);
        }

        public void Dash()
        {
            animator.SetBool(dash, playerController.isDashing);
        }

        public void WallSlidingAndJumping()
        {
            animator.SetBool(wallSliding, playerController.isWallSliding);
            animator.SetBool(wallJump, playerController.isWallJumping);
        }

        public void Hurt()
        {
            if (!isHurt)
            {
                isHurt = true;
                playerController.runMaxSpeed = 0; // Stop movement immediately
                animator.SetTrigger(hurt);
                playerController.move = Vector2.zero; // Ignore player movement input
            }
        }

        // Called by the animation event at the end of the Hurt animation
        public void EndHurt()
        {
            if (isHurt)
            {
                animator.SetTrigger(hurtEnded);
                isHurt = false;
                playerController.runMaxSpeed = playerController.originalRunMaxSpeed;
                Debug.Log("Hurt animation ended, transitioning to idle.");
            }
        }

        public void Death()
        {
            animator.SetTrigger(death);
        }
    }
}
