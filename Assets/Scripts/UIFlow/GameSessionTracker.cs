using TMPro;
using UnityEngine;

public class GameSessionTracker : MonoBehaviour
{
    [Header("Win Condition")]
    [SerializeField] private int targetWavesToWin = 3;

    [Header("Rewards")]
    [SerializeField] private int coinPerKill = 2;
    [SerializeField] private int coinPerWave = 40;
    [SerializeField] private int gemWinBonus = 3;

    [Header("Optional HUD")]
    [SerializeField] private TMP_Text waveProgressText;
    [SerializeField] private TMP_Text killProgressText;

    public int WavesCompleted { get; private set; }
    public int Kills { get; private set; }
    public bool IsVictory { get; private set; }

    public int CurrentRunCoinReward => (Kills * coinPerKill) + (WavesCompleted * coinPerWave);
    public int CurrentRunGemReward => IsVictory ? gemWinBonus : 0;

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
        GameEvents.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
        GameEvents.OnPlayerDied -= HandlePlayerDied;
    }

    private void Start()
    {
        RefreshHud();
    }

    private void HandleEnemyKilled(bool isBoss)
    {
        Kills++;
        RefreshHud();
    }

    private void HandleWaveCompleted(int waveNumber)
    {
        WavesCompleted = Mathf.Max(WavesCompleted, waveNumber);
        RefreshHud();

        if (WavesCompleted >= targetWavesToWin && !IsVictory)
        {
            IsVictory = true;
            GameEvents.RaiseGameWon();
        }
    }

    private void HandlePlayerDied()
    {
        IsVictory = false;
    }

    private void RefreshHud()
    {
        if (waveProgressText != null)
            waveProgressText.text = $"Dalga: {WavesCompleted}/{targetWavesToWin}";

        if (killProgressText != null)
            killProgressText.text = $"Öldürme: {Kills}";
    }

    public void ApplyRunRewardsToProfile()
    {
        MainMenuProgressData profile = MainMenuProgressService.Load();
        profile.coins += CurrentRunCoinReward;
        profile.gems += CurrentRunGemReward;
        profile.level = Mathf.Max(1, profile.level + (WavesCompleted / 2));
        MainMenuProgressService.Save(profile);
    }
}
