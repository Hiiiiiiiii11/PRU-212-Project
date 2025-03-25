using TMPro;
using UnityEngine;

namespace level2
{
    public class Portal : MonoBehaviour
    {
        private GameManager gameManager;
        [SerializeField] private TextMeshProUGUI messageText;
        private bool isPlayerNear = false;

        void Awake()
        {
            gameManager = FindFirstObjectByType<GameManager>();
            messageText.gameObject.SetActive(false);
        }

        void Update()
        {
            if (isPlayerNear && gameManager.crystal > 0)
            {
                gameManager.GameWin();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerNear = true;
                if(gameManager.crystal == 0)
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
