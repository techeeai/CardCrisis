using UnityEngine;
using TMPro;

public class WaveDirector : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject normalRobotPrefab;
    [SerializeField] private GameObject bossRobotPrefab;

    [Header("Spawn")]
    [SerializeField] private float spawnInterval = 0.7f;
    [SerializeField] private int maxAlive = 6;
    [SerializeField] private float spawnY = -5f;
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX = 2.5f;

    [Header("Spacing")]
    [SerializeField] private float minSpawnXDistance = 0.8f;
    private float lastSpawnX = 999f;

    [Header("Boss Look & Feel")]
    [SerializeField] private float bossScale = 1.8f;
    [SerializeField] private float bossSpeed = 0.8f;

    [Header("Boss Warning UI")]
    [SerializeField] private TMP_Text bossWarningText;
    [SerializeField] private float bossWarningDuration = 3f;

    private int wave = 1;

    private int aliveNormals = 0;
    private bool bossAlive = false;

    private int plannedNormalCount;
    private int spawnedNormals;
    private int maxNormalHpThisWave;

    private float spawnTimer;
    private float bossWarningTimer;

    private enum State { SpawningNormals, WaitingNormalsClear, BossWarning, BossAlive }
    private State state;
    private void Start()
    {
        if (bossWarningText != null)
            bossWarningText.gameObject.SetActive(false);

        BeginWave();
    }

    private void Update()
    {
        if (DifficultyManager.Instance == null) return;

        switch (state)
        {
            case State.SpawningNormals:
                TickSpawnNormals();
                break;

            case State.WaitingNormalsClear:
                if (aliveNormals <= 0)
                    StartBossWarning();
                break;

            case State.BossWarning:
                TickBossWarning();
                break;

            case State.BossAlive:
                if (!bossAlive)
                {
                    wave++;
                    BeginWave();
                }
                break;
        }
    }

    private void BeginWave()
    {
        plannedNormalCount = DifficultyManager.Instance.GetNormalCount(wave);
        spawnedNormals = 0;
        maxNormalHpThisWave = 0;
        spawnTimer = 0f;

        state = State.SpawningNormals;

        Debug.Log($"[WaveDirector] Wave {wave} başladı. Normal count: {plannedNormalCount}");
    }

    private void TickSpawnNormals()
    {
        if (spawnedNormals >= plannedNormalCount)
        {
            state = State.WaitingNormalsClear;
            return;
        }

        if (aliveNormals >= maxAlive) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnInterval) return;
        spawnTimer = 0f;

        SpawnOneNormal(spawnedNormals);
        spawnedNormals++;
    }
    private void SpawnOneNormal(int spawnIndex)
    {
        if (normalRobotPrefab == null) return;

        float x = Random.Range(minX, maxX);
        int tries = 0;
        while (Mathf.Abs(x - lastSpawnX) < minSpawnXDistance && tries < 10)
        {
            x = Random.Range(minX, maxX);
            tries++;
        }
        lastSpawnX = x;

        Vector3 pos = new Vector3(x, spawnY, 0f);
        GameObject z = Instantiate(normalRobotPrefab, pos, Quaternion.identity);

        int hp = DifficultyManager.Instance.GetNormalHpForSpawn(wave, spawnIndex);
        maxNormalHpThisWave = Mathf.Max(maxNormalHpThisWave, hp);

        // ✅ Debug: hp gerçekten artıyor mu?
        Debug.Log($"[WaveDirector] wave={wave} spawnIndex={spawnIndex} hp={hp}");

        Debug.Log($"[WD] Using DM = {DifficultyManager.Instance.name} id={DifficultyManager.Instance.GetInstanceID()} inc%={DifficultyManager.Instance.DebugSpawnInc()} spawnIndex={spawnIndex} hp={hp}");
        // ✅ EnemyStats root'ta değilse child'da da bul
        var stats = z.GetComponentInChildren<EnemyStats>(true);
        if (stats != null)
        {
            stats.SetMaxHp(hp);
            stats.OnDied += HandleNormalDied;
        }
        else
        {
            Debug.LogError("[WaveDirector] EnemyStats bulunamadı! Prefab root/child kontrol et: " + z.name);
        }

        aliveNormals++;
    }

    private void HandleNormalDied(EnemyStats e)
    {
        aliveNormals--;
        if (aliveNormals < 0) aliveNormals = 0;
    }

    private void StartBossWarning()
    {
        bossWarningTimer = 0f;
        state = State.BossWarning;

        if (bossWarningText != null)
        {
            bossWarningText.text = "DİKKAT! BOSS GELİYOR!";
            bossWarningText.gameObject.SetActive(true);
        }
    }

    private void TickBossWarning()
    {
        bossWarningTimer += Time.deltaTime;
        if (bossWarningTimer < bossWarningDuration) return;

        if (bossWarningText != null)
            bossWarningText.gameObject.SetActive(false);

        bool spawned = SpawnBoss();
        state = spawned ? State.BossAlive : State.SpawningNormals;

        if (!spawned)
            Debug.LogError("[WaveDirector] Boss prefab bağlı değil! Inspector’dan bossRobotPrefab bağla.");
    }

    private bool SpawnBoss()
    {
        if (bossRobotPrefab == null) return false;

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        GameObject boss = Instantiate(bossRobotPrefab, pos, Quaternion.identity);
        boss.transform.localScale = Vector3.one * bossScale;

        int bossHp = DifficultyManager.Instance.GetBossHpFromMaxNormal(maxNormalHpThisWave);

        var stats = boss.GetComponentInChildren<EnemyStats>(true);
        if (stats != null)
        {
            stats.SetMaxHp(bossHp);
            stats.OnDied += HandleBossDied;
        }
        else
        {
            Debug.LogError("[WaveDirector] Boss EnemyStats bulunamadı! Prefab root/child kontrol et: " + boss.name);
        }

        var move = boss.GetComponentInChildren<RobotMovement>(true);
        if (move != null) move.speed = bossSpeed;

        bossAlive = true;
        Debug.Log($"[WaveDirector] Boss çıktı! HP: {bossHp}");
        return true;
    }

    private void HandleBossDied(EnemyStats e)
    {
        bossAlive = false;
        Debug.Log($"[WaveDirector] Boss öldü. Wave {wave} bitti.");
    }
}