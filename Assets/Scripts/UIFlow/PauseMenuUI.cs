using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private bool allowEscapeKey = true;

    private bool isPaused;

    private void Awake()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null) pauseButton.onClick.AddListener(Pause);
        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (restartButton != null) restartButton.onClick.AddListener(Restart);
        if (menuButton != null) menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void Update()
    {
        if (!allowEscapeKey) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();
    }

    public void Resume()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.StartGameplayFromMenu();
    }

    private void Restart()
    {
        isPaused = false;
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }

    private void ReturnToMenu()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();
    }
}
