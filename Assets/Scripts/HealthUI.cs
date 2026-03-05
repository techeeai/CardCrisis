using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        if (healthText == null)
            healthText = GetComponent<TMP_Text>();

        if (healthText == null)
            healthText = GetComponentInChildren<TMP_Text>(true);
    }

    private void OnEnable()
    {
        GameEvents.OnHealthChanged += UpdateText;
    }

    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= UpdateText;
    }

    private void UpdateText(int currentHealth)
    {
        if (healthText == null)
            return;

        healthText.SetText("CAN: {0}", currentHealth);
    }
}
