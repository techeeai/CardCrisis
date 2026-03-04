using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerDied += Show;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDied -= Show;
    }

    private void Show()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}