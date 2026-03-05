using UnityEngine;
using System;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;
    [SerializeField] private TMP_Text hpText;

    private int currentHp;
    private bool isDead;
    private bool isBoss;

    public event Action<EnemyStats> OnDied;

    private void Awake()
    {
        currentHp = maxHp;
        UpdateHpText();
    }

    public void SetMaxHp(int value)
    {
        maxHp = Mathf.Max(1, value);
        currentHp = maxHp;
        UpdateHpText();
    }

    public void SetIsBoss(bool value)
    {
        isBoss = value;
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHp -= Mathf.Max(0, dmg);
        UpdateHpText();

        if (currentHp <= 0)
            Die();
    }

    private void UpdateHpText()
    {
        if (hpText != null)
            hpText.text = currentHp.ToString();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDied?.Invoke(this);
        GameEvents.RaiseEnemyKilled(isBoss);
        Destroy(gameObject);
    }
}
