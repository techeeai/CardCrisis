using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int health = 10;

    private bool isDead = false;

    private void Start()
    {
        // Oyun başında UI güncellensin
        GameEvents.RaiseHealthChanged(health);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Robot") || other.name.Contains("Robot"))
        {
            TakeDamage(1);

            var stats = other.GetComponent<EnemyStats>();
            if (stats != null)
                stats.Die();
            else
                Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log("[PlayerHealth] RaiseHealthChanged -> " + health);
        GameEvents.RaiseHealthChanged(health);

        if (health <= 0)
        {
            isDead = true;
            GameEvents.RaisePlayerDied();
        }
    }
}