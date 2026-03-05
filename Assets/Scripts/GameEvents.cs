using System;

public static class GameEvents
{
    public static event Action<int> OnHealthChanged;
    public static event Action OnPlayerDied;
    public static event Action<bool> OnEnemyKilled;
    public static event Action<int> OnWaveCompleted;
    public static event Action OnGameWon;

    public static int CurrentHealth { get; private set; } = -1;

    public static void RaiseHealthChanged(int newHealth)
    {
        CurrentHealth = newHealth;
        OnHealthChanged?.Invoke(newHealth);
    }

    public static void RaisePlayerDied() => OnPlayerDied?.Invoke();

    public static void RaiseEnemyKilled(bool isBoss) => OnEnemyKilled?.Invoke(isBoss);

    public static void RaiseWaveCompleted(int waveNumber) => OnWaveCompleted?.Invoke(waveNumber);

    public static void RaiseGameWon() => OnGameWon?.Invoke();
}
