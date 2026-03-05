using TMPro;
using UnityEngine;

public class QuestPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TMP_Text waveQuestText;
    [SerializeField] private TMP_Text killQuestText;
    [SerializeField] private int waveQuestTarget = 3;
    [SerializeField] private int killQuestTarget = 20;

    private int wavesCompleted;
    private int kills;

    private void Awake()
    {
        if (questPanel != null)
            questPanel.SetActive(false);

        RefreshTexts();
    }

    private void OnEnable()
    {
        GameEvents.OnWaveCompleted += HandleWaveCompleted;
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnWaveCompleted -= HandleWaveCompleted;
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    public void Toggle()
    {
        if (questPanel == null)
            return;

        questPanel.SetActive(!questPanel.activeSelf);
    }

    private void HandleWaveCompleted(int wave)
    {
        wavesCompleted = Mathf.Max(wavesCompleted, wave);
        RefreshTexts();
    }

    private void HandleEnemyKilled(bool isBoss)
    {
        kills++;
        RefreshTexts();
    }

    private void RefreshTexts()
    {
        if (waveQuestText != null)
        {
            bool done = wavesCompleted >= waveQuestTarget;
            waveQuestText.text = done
                ? $"✓ Görev 1: {waveQuestTarget} dalga tamamla"
                : $"Görev 1: {wavesCompleted}/{waveQuestTarget} dalga tamamla";
        }

        if (killQuestText != null)
        {
            bool done = kills >= killQuestTarget;
            killQuestText.text = done
                ? $"✓ Görev 2: {killQuestTarget} robot öldür"
                : $"Görev 2: {kills}/{killQuestTarget} robot öldür";
        }
    }
}
