using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }
    public bool IsGameplayActive { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= HandlePlayerDied;
    }

    private void Start()
    {
        ResetGameState();
    }

    private void HandlePlayerDied()
    {
        if (IsGameOver) return;

        IsGameOver = true;
        IsGameplayActive = false;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnterMainMenu()
    {
        IsGameplayActive = false;
        IsGameOver = false;
        Time.timeScale = 0f;
    }

    public void StartGameplayFromMenu()
    {
        IsGameplayActive = true;
        IsGameOver = false;
        Time.timeScale = 1f;
    }

    private void ResetGameState()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        IsGameplayActive = true;
    }
}
