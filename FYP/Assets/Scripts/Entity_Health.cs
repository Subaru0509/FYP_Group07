using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity entity;

    [Header("Health")]
    [SerializeField] protected float maxHP = 100f;
    [SerializeField] protected float currentHP = 0f;
    [SerializeField] protected bool isDead;

    public event Action<float, float> OnHealthChanged;

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2f);

    [Header("On Heavy Damage Knockback")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = .3f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);

    [Header("UI Binding (Optional)")]
    [SerializeField] private Image healthFillImage;

    private Coroutine healthBarCoroutine;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();

        if (currentHP <= 0f || currentHP > maxHP)
            currentHP = maxHP;

        OnHealthChanged += UpdateHealthBar;
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        float duration = CalculateDuration(damage);
        Vector2 knockback = CalculateKnockback(damage, damageDealer);

        entityVFX?.PlayOnDamageVfx();
        entity?.ReciveKnockback(knockback, duration);
        ReduceHP(damage);
    }

    protected void ReduceHP(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0f);

        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        entity?.EntityDeath();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;
        knockback.x *= direction;
        return knockback;
    }

    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage) => (maxHP > 0f) && (damage / maxHP >= heavyDamageThreshold);

    private void UpdateHealthBar(float current, float max)
    {
        if (healthFillImage != null)
        {
            float targetFill = current / max;

            if (healthBarCoroutine != null)
                StopCoroutine(healthBarCoroutine);

            healthBarCoroutine = StartCoroutine(SmoothFill(targetFill));
        }
    }

    private IEnumerator SmoothFill(float targetFill)
    {
        float duration = 0.3f; 
        float elapsed = 0f;
        float startFill = healthFillImage.fillAmount;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            healthFillImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            yield return null;
        }

        healthFillImage.fillAmount = targetFill; 
    }

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;
    public bool IsDead => isDead;
}
