using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Entity_VFX entityVFX;
    private Entity entity;
    private Entity_Stats stats;

    [Header("Health")]
    [SerializeField] protected float currentHP;
    [SerializeField] protected bool isDead;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2f);

    [Header("On Heavy Damage Knockback")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = .3f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();

        if (currentHP <= 0f || currentHP > stats.GetMaxHealth())
            currentHP = stats.GetMaxHealth();

        if (healthBar != null)
            healthBar.SetMaxHealth(stats.GetMaxHealth());
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return;

        // 新增：格挡系统 - 先消耗格挡值
        if (entity != null && entity.currentBlock > 0)
        {
            int blockedDamage = Mathf.Min(entity.currentBlock, (int)damage);
            entity.currentBlock -= blockedDamage;
            damage -= blockedDamage;
            Debug.Log($"{entity.name} 的格挡吸收了 {blockedDamage} 点伤害，剩余格挡: {entity.currentBlock}");
        }

        // 如果伤害被完全格挡，不执行后续逻辑
        if (damage <= 0)
        {
            Debug.Log($"{entity.name} 的攻击被完全格挡！");
            return;
        }

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

        if (healthBar != null)
            healthBar.SetHealth(currentHP, stats.GetMaxHealth());

        if (currentHP <= 0f)
            Die();
    }

    public void IncreaseHealth(float amount)
    {
        if (isDead) return;

        currentHP += amount;
        currentHP = Mathf.Min(currentHP, stats.GetMaxHealth());

        if (healthBar != null)
            healthBar.SetHealth(currentHP, stats.GetMaxHealth());
    }

    public void SetHealth(float value)
    {
        if (isDead) return;

        currentHP = Mathf.Clamp(value, 0f, stats.GetMaxHealth());

        if (healthBar != null)
            healthBar.SetHealth(currentHP, stats.GetMaxHealth());

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

    private bool IsHeavyDamage(float damage) => (stats.GetMaxHealth() > 0f) && (damage / stats.GetMaxHealth() >= heavyDamageThreshold);

    public float CurrentHP => currentHP;
    public float MaxHP => stats.GetMaxHealth();
    public bool IsDead => isDead;
}
