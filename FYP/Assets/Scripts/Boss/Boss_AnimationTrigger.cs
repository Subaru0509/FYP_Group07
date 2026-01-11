using UnityEngine;

/// <summary>
/// 挂载在Boss动画对象上，用于动画事件回调
/// </summary>
public class Boss_AnimationTrigger : MonoBehaviour
{
    private Boss boss;

    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }

    /// <summary>
    /// 动画触发器 - 在动画关键帧调用
    /// </summary>
    private void AnimationTrigger()
    {
        boss.CurrentStateAnimationTrigger();
    }

    /// <summary>
    /// 攻击触发器 - 在攻击动画的伤害帧调用
    /// </summary>
    private void AttackTrigger()
    {
        // 获取攻击范围内的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            boss.transform.position + new Vector3(boss.facingDir * 1.5f, 0, 0),
            1.5f
        );

        foreach (var hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                // 对玩家造成伤害（使用当前攻击的伤害值）
                var playerHealth = hit.GetComponent<Entity_Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(boss.currentAttackDamage, boss.transform);
                }

                // 击退玩家（重击有更强的击退）
                var player = hit.GetComponent<Player>();
                if (player != null)
                {
                    float knockbackMultiplier = 1f + (boss.currentAttackIndex * 0.5f); // 攻击越强击退越大
                    Vector2 knockback = new Vector2(boss.facingDir * 5f * knockbackMultiplier, 3f * knockbackMultiplier);
                    player.ReciveKnockback(knockback, 0.2f);
                }
            }
        }
    }

    /// <summary>
    /// 开启反击窗口
    /// </summary>
    private void EnableCounterWindow()
    {
        boss.EnableCounterWindow(true);
    }

    /// <summary>
    /// 关闭反击窗口
    /// </summary>
    private void DisableCounterWindow()
    {
        boss.EnableCounterWindow(false);
    }
}
