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
    [SerializeField] private PlayerHealth playerHealth;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag)) return;

        int id = other.gameObject.GetInstanceID();
        if (processedInstanceIds.Contains(id)) return;
        processedInstanceIds.Add(id);

        if (playerHealth != null)
            playerHealth.TakeDamage(damage);

        var stats = other.GetComponent<EnemyStats>();
        if (stats != null)
            stats.Die();
        else
            Destroy(other.gameObject);
    }
}
