using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameWinUI;
    private bool isGameOver = false;
    private bool isGameWin = false;
    private int clockPragment = 0;
    [SerializeField] private TextMeshProUGUI clockText;
    public TextMeshProUGUI NeedPragmentText;

    void Start()
    {

        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        Time.timeScale = 1;
        isGameOver = false;
        NeedPragmentText.gameObject.SetActive(false); // Reset lại biến isGameOver mỗi khi bắt đầu game
    }

    public void GameOver()
    {
        if (isGameOver) return; // Ngăn việc gọi lại GameOver nhiều lần

        isGameOver = true;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }
    public void GameWin()
    {
        if (isGameWin) return; // Ngăn việc gọi lại GameOver nhiều lần

        isGameWin = true;
        Time.timeScale = 0;
        gameWinUI.SetActive(true);
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
    public void AddPragment(int pragment)
    {
        clockPragment += pragment;
        UpdatePragment();
    }
    private void UpdatePragment()
    {
        clockText.text = clockPragment.ToString();
    }
    public int GetClockPragment()
    {
        return clockPragment;
    }
    public void ShowNeedPragmentText()
    {
        NeedPragmentText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideNeedPragmentText)); // Hủy nếu đang chạy trước đó
        Invoke(nameof(HideNeedPragmentText), 2f); // Ẩn sau 2 giây
    }

    private void HideNeedPragmentText()
    {
        NeedPragmentText.gameObject.SetActive(false);
    }

}
