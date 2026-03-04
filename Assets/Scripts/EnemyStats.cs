using UnityEngine;
using System;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;
    private int currentHp;

    [SerializeField] private TMP_Text hpText; // 👈 EKLENDİ

    public event Action<EnemyStats> OnDied;

    private void Awake()
    {
        currentHp = maxHp;
        UpdateHpText(); // 👈 EKLENDİ
    }

    public void SetMaxHp(int value)
    {
        maxHp = Mathf.Max(1, value);
        currentHp = maxHp;
        UpdateHpText(); // 👈 EKLENDİ
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= Mathf.Max(0, dmg);
        UpdateHpText(); // 👈 EKLENDİ

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
        OnDied?.Invoke(this);
        Destroy(gameObject);
    }
}