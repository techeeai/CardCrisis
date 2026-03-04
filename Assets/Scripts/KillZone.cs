using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillZone : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string targetTag = "Robot";

    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth; // Inspector’dan bağla

    // Aynı Robot birden fazla tetiklenirse iki kez hasar vermesin
    private readonly HashSet<int> processedInstanceIds = new HashSet<int>();

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
            Debug.LogWarning("KillZone: PlayerHealth bulunamadı. Üst duvar robotu yok edecek ama can düşürmeyecek.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;

        int id = other.gameObject.GetInstanceID();
        if (processedInstanceIds.Contains(id)) return;
        processedInstanceIds.Add(id);

        // Robot kaçtı => can düş
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);

        // ✅ DEĞİŞİKLİK BURADA:
        // WaveDirector sayacı doğru azalsın diye (OnDied tetiklensin)
        var stats = other.GetComponent<EnemyStats>();
        if (stats != null)
            stats.Die();
        else
            Destroy(other.gameObject);
    }
}