using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;

    void Start()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1;
        isGameOver = false;  // Reset lại biến isGameOver mỗi khi bắt đầu game
    }

    public void GameOver()
    {
        if (isGameOver) return; // Ngăn việc gọi lại GameOver nhiều lần

        isGameOver = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1;
        gameOverUI.SetActive(false); // Ẩn UI khi restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load lại scene hiện tại
    }
    public void GotoMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
}
