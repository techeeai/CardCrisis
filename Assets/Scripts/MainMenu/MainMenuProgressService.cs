using System;
using UnityEngine;

[Serializable]
public class MainMenuProgressData
{
    public string profileName = "Player";
    public int level = 1;
    public int coins = 250;
    public int gems = 30;
    public long lastDailyRewardUtcTicks;
}

public static class MainMenuProgressService
{
    private const string SaveKey = "main_menu_progress_v1";
    private static MainMenuProgressData cached;

    public static MainMenuProgressData Load()
    {
        if (cached != null)
            return cached;

        if (!PlayerPrefs.HasKey(SaveKey))
        {
            cached = CreateDefault();
            Save(cached);
            return cached;
        }

        string json = PlayerPrefs.GetString(SaveKey);
        cached = JsonUtility.FromJson<MainMenuProgressData>(json);

        if (cached == null)
        {
            cached = CreateDefault();
            Save(cached);
        }

        return cached;
    }

    public static void Save(MainMenuProgressData data)
    {
        cached = data;
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static bool TryClaimDailyReward(int rewardCoins, int rewardGems, TimeSpan cooldown, out TimeSpan remaining)
    {
        MainMenuProgressData data = Load();

        DateTime now = DateTime.UtcNow;
        DateTime lastClaim = data.lastDailyRewardUtcTicks > 0
            ? new DateTime(data.lastDailyRewardUtcTicks, DateTimeKind.Utc)
            : DateTime.MinValue;

        TimeSpan elapsed = now - lastClaim;
        if (elapsed < cooldown)
        {
            remaining = cooldown - elapsed;
            return false;
        }

        data.coins += Mathf.Max(0, rewardCoins);
        data.gems += Mathf.Max(0, rewardGems);
        data.lastDailyRewardUtcTicks = now.Ticks;
        Save(data);

        remaining = TimeSpan.Zero;
        return true;
    }

    public static TimeSpan GetDailyRemaining(TimeSpan cooldown)
    {
        MainMenuProgressData data = Load();
        if (data.lastDailyRewardUtcTicks <= 0)
            return TimeSpan.Zero;

        DateTime lastClaim = new DateTime(data.lastDailyRewardUtcTicks, DateTimeKind.Utc);
        TimeSpan elapsed = DateTime.UtcNow - lastClaim;
        return elapsed >= cooldown ? TimeSpan.Zero : cooldown - elapsed;
    }

    private static MainMenuProgressData CreateDefault()
    {
        return new MainMenuProgressData();
    }
}
