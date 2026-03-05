using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject splashPanel;
    [SerializeField] private float splashDuration = 1.4f;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Header UI")]
    [SerializeField] private TMP_Text profileNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemText;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button cardsButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button dailyRewardButton;

    [Header("Secondary Panels")]
    [SerializeField] private GameObject cardsPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Daily Reward")]
    [SerializeField] private TMP_Text dailyRewardStatusText;
    [SerializeField] private int dailyCoinReward = 75;
    [SerializeField] private int dailyGemReward = 5;
    [SerializeField] private float dailyCooldownHours = 24f;

    private MainMenuProgressData profile;
    private float splashTimer;

    private void Awake()
    {
        profile = MainMenuProgressService.Load();

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        SetPanelState(cardsPanel, false);
        SetPanelState(inventoryPanel, false);
        SetPanelState(settingsPanel, false);

        if (splashPanel != null)
            splashPanel.SetActive(true);

        if (playButton != null) playButton.onClick.AddListener(Play);
        if (cardsButton != null) cardsButton.onClick.AddListener(OpenCards);
        if (inventoryButton != null) inventoryButton.onClick.AddListener(OpenInventory);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (dailyRewardButton != null) dailyRewardButton.onClick.AddListener(ClaimDailyReward);

        if (GameManager.Instance != null)
            GameManager.Instance.EnterMainMenu();

        RefreshHeader();
        RefreshDailyRewardStatus();
    }

    private void Update()
    {
        if (splashPanel != null && splashPanel.activeSelf)
        {
            splashTimer += Time.unscaledDeltaTime;
            if (splashTimer >= splashDuration)
            {
                splashPanel.SetActive(false);
                if (mainMenuPanel != null)
                    mainMenuPanel.SetActive(true);
            }
        }

        RefreshDailyRewardStatus();
    }

    private void Play()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        SetPanelState(cardsPanel, false);
        SetPanelState(inventoryPanel, false);
        SetPanelState(settingsPanel, false);

        if (GameManager.Instance != null)
            GameManager.Instance.StartGameplayFromMenu();
    }

    private void OpenCards() => OpenSubPanel(cardsPanel);
    private void OpenInventory() => OpenSubPanel(inventoryPanel);
    private void OpenSettings() => OpenSubPanel(settingsPanel);

    private void OpenSubPanel(GameObject target)
    {
        SetPanelState(cardsPanel, target == cardsPanel);
        SetPanelState(inventoryPanel, target == inventoryPanel);
        SetPanelState(settingsPanel, target == settingsPanel);
    }

    private void ClaimDailyReward()
    {
        TimeSpan cooldown = TimeSpan.FromHours(dailyCooldownHours);
        bool claimed = MainMenuProgressService.TryClaimDailyReward(
            dailyCoinReward,
            dailyGemReward,
            cooldown,
            out _);

        if (claimed)
            profile = MainMenuProgressService.Load();

        RefreshHeader();
        RefreshDailyRewardStatus();
    }

    private void RefreshHeader()
    {
        if (profileNameText != null)
            profileNameText.text = profile.profileName;

        if (levelText != null)
            levelText.text = $"Lv. {profile.level}";

        if (coinText != null)
            coinText.text = profile.coins.ToString();

        if (gemText != null)
            gemText.text = profile.gems.ToString();
    }

    private void RefreshDailyRewardStatus()
    {
        if (dailyRewardStatusText == null)
            return;

        TimeSpan cooldown = TimeSpan.FromHours(dailyCooldownHours);
        TimeSpan remaining = MainMenuProgressService.GetDailyRemaining(cooldown);
        bool available = remaining <= TimeSpan.Zero;

        if (dailyRewardButton != null)
            dailyRewardButton.interactable = available;

        dailyRewardStatusText.text = available
            ? "Günlük ödül hazır!"
            : $"Sonraki ödül: {remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00}";
    }

    private void SetPanelState(GameObject panel, bool active)
    {
        if (panel != null)
            panel.SetActive(active);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
