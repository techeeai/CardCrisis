using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }

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
        Time.timeScale = 0f;

    }

    public void RestartGame()
    {
        // TimeScale 0 kalmış olabilir, önce düzelt
        Time.timeScale = 1f;

        // Aynı sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGameState()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
    }
}
