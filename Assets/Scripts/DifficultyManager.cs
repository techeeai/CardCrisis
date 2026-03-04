using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Normal Robot HP")]
    [SerializeField] private int baseHpAtWave1 = 10;
    [SerializeField] private float waveStartIncreasePercent = 10f;
    [SerializeField] private float spawnIncreasePercent = 20f;

    [Header("Counts")]
    [SerializeField] private int baseCount = 20;
    [SerializeField] private int extraPerGroup = 5;
    [SerializeField] private int wavesPerGroup = 5;

    [Header("Boss")]
    [SerializeField] private float bossHpMultiplier = 5f;

    private void Awake()
    {
        Debug.Log($"[DM] Awake on {name} | spawnInc%={spawnIncreasePercent}");

        if (Instance != null && Instance != this)
        {
            Debug.Log($"[DM] DUPLICATE DESTROY: {name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log($"[DM] Instance SET => {name} | spawnInc%={spawnIncreasePercent}");
    }

    public int GetNormalCount(int wave)
    {
        int group = (wave - 1) / wavesPerGroup;
        return baseCount + group * extraPerGroup;
    }

    private int GetWaveMinHp(int wave)
    {
        if (wave <= 1) return baseHpAtWave1;

        int prevWave = wave - 1;
        int prevCount = GetNormalCount(prevWave);
        int prevMaxHp = GetNormalHpForSpawn(prevWave, prevCount - 1);

        float multiplier = 1f + (waveStartIncreasePercent / 100f);
        return Mathf.RoundToInt(prevMaxHp * multiplier);
    }
    public float DebugSpawnInc() => spawnIncreasePercent;

    public int GetNormalHpForSpawn(int wave, int spawnIndex)
    {
        int hp = GetWaveMinHp(wave);

        float inc = spawnIncreasePercent / 100f;
        for (int i = 0; i < spawnIndex; i++)
            hp = Mathf.CeilToInt(hp + (hp * inc));
        return hp;
    }

    public int GetBossHpFromMaxNormal(int maxNormalHpThisWave)
    {
        return Mathf.CeilToInt(maxNormalHpThisWave * bossHpMultiplier);
    }
}