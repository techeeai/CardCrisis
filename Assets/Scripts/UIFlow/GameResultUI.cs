using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private GameSessionTracker sessionTracker;

    private bool shown;

    private void Awake()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (restartButton != null) restartButton.onClick.AddListener(Restart);
        if (menuButton != null) menuButton.onClick.AddListener(ReturnToMenu);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDied += ShowLose;
        GameEvents.OnGameWon += ShowWin;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= ShowLose;
        GameEvents.OnGameWon -= ShowWin;
    }

    private void ShowWin()
    {
        ShowResult(true);
    }

    private void ShowLose()
    {
        ShowResult(false);
    }

    private void ShowResult(bool isWin)
    {
        if (shown || sessionTracker == null)
            return;

        shown = true;
        sessionTracker.ApplyRunRewardsToProfile();

        if (titleText != null)
            titleText.text = isWin ? "KAZANDIN" : "KAYBETTİN";

        if (summaryText != null)
        {
            summaryText.text =
                $"Dalga: {sessionTracker.WavesCompleted}\n" +
                $"Öldürme: {sessionTracker.Kills}\n" +
                $"Ödül Coin: +{sessionTracker.CurrentRunCoinReward}\n" +
                $"Ödül Gem: +{sessionTracker.CurrentRunGemReward}";
        }

        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();
    }

    private void Restart()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }

    private void ReturnToMenu()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();
    }
}
