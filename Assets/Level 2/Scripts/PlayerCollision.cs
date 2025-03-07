using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace level2
{
    public class PlayerCollision : MonoBehaviour
    {
        private GameManager gameManager;

        private void Awake()
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.CompareTag("Chest"))
            {
                Animator chestAnimator = collision.GetComponent<Animator>();

                if (gameManager.CanOpenChest())
                {
                    chestAnimator.SetTrigger("Open");
                    StartCoroutine(DestroyAfterAnimation(collision.gameObject, chestAnimator));
                    gameManager.ReduceKey();
                    gameManager.AddFragment();
                }
            }
            else if (collision.CompareTag("Key"))
            {
                gameManager.AddKey();
                Destroy(collision.gameObject);
            }
            else if (collision.CompareTag("Gate"))
            {
                if (gameManager.CanOpenGate())
                {
                    Debug.Log("You have finished the level!");
                };
            }
            else if (collision.CompareTag("Trap"))
            {
                gameObject.GetComponent<PlayerController>().enabled = false;
                Animator playerAnimator = GetComponent<Animator>();
                playerAnimator.SetTrigger("Death");
                StartCoroutine(DestroyAfterAnimation(GameObject.FindGameObjectsWithTag("Player")[0], playerAnimator));
                StartCoroutine(GameOver());
            }


        }

        private IEnumerator GameOver()
        {
            yield return new WaitForSeconds(2);
            gameManager.GameOver();
        }

        private IEnumerator DestroyAfterAnimation(GameObject gameObject, Animator animator)
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}