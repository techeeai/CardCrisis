using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        // 1) Önce aynı objede var mı?
        if (healthText == null)
            healthText = GetComponent<TMP_Text>();

        // 2) Yoksa çocuklarda da ara (çok sık: HealthUI parent'ta, TMP child'ta oluyor)
        if (healthText == null)
            healthText = GetComponentInChildren<TMP_Text>(true);

        // 3) Hâlâ yoksa sahnede TMP arayıp raporla
        if (healthText == null)
        {
            var all = FindObjectsOfType<TMP_Text>(true);
            Debug.LogError($"[HealthUI] TMP_Text bulunamadı! Bu objede TMP yok. Sahnedeki TMP sayısı: {all.Length}. " +
                           $"HealthUI GameObject: {gameObject.name}");
            for (int i = 0; i < all.Length; i++)
                Debug.Log($"[HealthUI] TMP[{i}] = {GetPath(all[i].transform)}");
        }
        else
        {
            Debug.Log($"[HealthUI] Bağlanan TMP: {GetPath(healthText.transform)}");
        }
    }

    private void OnEnable()
    {
        Debug.Log("[HealthUI] OnEnable çalıştı");
        GameEvents.OnHealthChanged += UpdateText;
    }

    private void OnDisable()
    {
        Debug.Log("[HealthUI] OnDisable -> event unsubscribe");
        GameEvents.OnHealthChanged -= UpdateText;
    }

    private void UpdateText(int currentHealth)
    {
        Debug.Log($"[HealthUI] UpdateText geldi: {currentHealth}");

        if (healthText == null)
        {
            Debug.LogError("[HealthUI] healthText NULL! Inspector’dan bağla ya da script’i doğru objeye koy.");
            return;
        }

        healthText.SetText("CAN: {0}", currentHealth);
    }

    private static string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}