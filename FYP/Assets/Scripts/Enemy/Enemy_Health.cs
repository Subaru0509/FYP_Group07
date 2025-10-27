using UnityEngine;

public class Enemy_Health : Entity_Health
{
    [SerializeField] private HealthBar healthBar;

    protected override void Awake()
    {
        base.Awake();
        if (healthBar != null)
        {
            OnHealthChanged += healthBar.SetHealth;
            healthBar.SetHealth(CurrentHP, MaxHP);
        }
    }
}
