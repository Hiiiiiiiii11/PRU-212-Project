using TMPro;
using UnityEngine;

namespace level3
{
    public class Door : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField] private TextMeshProUGUI messageText;
        private bool isPlayerNear = false; // Track player proximity

        void Awake()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            messageText.gameObject.SetActive(false);
        }

        void Update()
        {
            // Check if the player is near and presses 'E'
            if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
            {
                TryOpenDoor();
            }
        }

        private void TryOpenDoor()
        {
            if (gameManager != null)
            {
                if (gameManager.HasKey())
                {
                    gameManager.UseKey();
                    messageText.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
                else
                {
                    messageText.text = "You do not have a key!";
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerNear = true;
                messageText.text = "Press E to open";
                messageText.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerNear = false;
                messageText.gameObject.SetActive(false);
            }
        }
    }
}
