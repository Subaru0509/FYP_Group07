using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image healthFillImage; 
    private Entity_Health entityHealth;

    public void Initialize(Entity_Health health)
    {
        entityHealth = health;
        entityHealth.OnHealthChanged += UpdateHealthBar;
        UpdateHealthBar(health.CurrentHP, health.MaxHP); 
    }

    private void UpdateHealthBar(float currentHP, float maxHP)
    {
        float fillAmount = currentHP / maxHP;
        healthFillImage.fillAmount = fillAmount;
    }

    private void OnDestroy()
    {
        if (entityHealth != null)
            entityHealth.OnHealthChanged -= UpdateHealthBar;
    }
}
