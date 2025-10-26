using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Health : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity entity;

    [SerializeField]protected float maxHP = 100;
    [SerializeField]protected bool isDead;

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
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if(isDead)
            return;

        float duration = CalculateDuration(damage);
        Vector2 knockback = CalculateKnockback(damage, damageDealer);

        entityVFX?.PlayOnDamageVfx();
        entity?.ReciveKnockback(knockback, duration);
        ReduceHP(damage);
    }

    protected void ReduceHP(float damage)
    {
        maxHP -= damage;

        if (maxHP < 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {

        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }

    private bool IsHeavyDamage(float damage) => damage / maxHP > heavyDamageThreshold;
}
