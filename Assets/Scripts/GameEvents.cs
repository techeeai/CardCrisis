using System;

public static class GameEvents
{
    public static event Action<int> OnHealthChanged;
    public static event Action OnPlayerDied;

    // UI event kaçırsa bile son değeri okuyabilsin
    public static int CurrentHealth { get; private set; } = -1;

    public static void RaiseHealthChanged(int newHealth)
    {
        CurrentHealth = newHealth;
        OnHealthChanged?.Invoke(newHealth);
    }

    public static void RaisePlayerDied()
        => OnPlayerDied?.Invoke();
}