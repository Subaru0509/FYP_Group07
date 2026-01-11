using UnityEngine;

public class BossState : EntityState
{
    protected Boss boss;

    public BossState(Boss boss, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.boss = boss;
        anim = boss.anim;
        rb = boss.rb;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        float battleAnimSpeedMultiplier = boss.battleMoveSpeed / boss.moveSpeed;

        anim.SetFloat("battleAnimSpeedMultiplier", battleAnimSpeedMultiplier);
        anim.SetFloat("moveAnimSpeedMultiplier", boss.moveAnimSpeedMultiplier);
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }

    /// <summary>
    /// 获取到玩家的方向 (1 或 -1)
    /// </summary>
    protected int DirectionToPlayer()
    {
        if (boss.player == null) return boss.facingDir;
        
        return boss.player.position.x > boss.transform.position.x ? 1 : -1;
    }

    /// <summary>
    /// 获取到玩家的距离
    /// </summary>
    protected float DistanceToPlayer()
    {
        if (boss.player == null) return float.MaxValue;
        
        return Vector2.Distance(boss.transform.position, boss.player.position);
    }

    /// <summary>
    /// 检查是否在攻击范围内
    /// </summary>
    protected bool WithinAttackRange()
    {
        return DistanceToPlayer() < boss.attackDistance;
    }

    /// <summary>
    /// 检查是否需要后退
    /// </summary>
    protected bool ShouldRetreat()
    {
        return DistanceToPlayer() < boss.minRetreatDistance;
    }
}
