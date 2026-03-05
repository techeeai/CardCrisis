using UnityEngine;
using UnityEngine.UI;

public class PreBattleDeckConfirmUI : MonoBehaviour
{
    [SerializeField] private GameObject preBattlePanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private bool autoOpenAtStart = false;

    private void Awake()
    {
        if (confirmButton != null)
            confirmButton.onClick.AddListener(ConfirmDeck);

        if (backToMenuButton != null)
            backToMenuButton.onClick.AddListener(BackToMenu);

        if (preBattlePanel != null)
            preBattlePanel.SetActive(false);
    }

    private void Start()
    {
        if (autoOpenAtStart)
            Open();
    }

    public void Open()
    {
        if (preBattlePanel != null)
            preBattlePanel.SetActive(true);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();
    }

    public void ConfirmDeck()
    {
        if (preBattlePanel != null)
            preBattlePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.StartGameplayFromMenu();
    }

    private void BackToMenu()
    {
        if (preBattlePanel != null)
            preBattlePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();
    }
}
