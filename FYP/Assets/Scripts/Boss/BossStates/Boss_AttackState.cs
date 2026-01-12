using UnityEngine;

public class Boss_AttackState : BossState
{
    private int attackIndex;

    public Boss_AttackState(Boss boss, StateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        // 根据概率选择攻击类型（必须在 base.Enter() 之前设置，否则 Blend Tree 无法正确选择动画）
        attackIndex = SelectAttackByProbability();
        boss.currentAttackIndex = attackIndex;
        
        // 设置对应的伤害值
        boss.currentAttackDamage = GetAttackDamage(attackIndex);
        
        // 先设置 comboIndex，再调用 base.Enter() 触发动画
        anim.SetFloat("comboIndex", attackIndex);
        
        base.Enter();

        // 面向玩家
        boss.HandleFlip(DirectionToPlayer());
        boss.SetVelocity(0, 0);

        // 增加攻击计数
        boss.IncrementAttackCounter();

        // 攻击开始时开启反击窗口（攻击准备阶段可以被格挡）
        boss.EnableCounterWindow(true);
    }

    /// <summary>
    /// 根据概率选择攻击类型
    /// </summary>
    private int SelectAttackByProbability()
    {
        float totalChance = boss.attack1Chance + boss.attack2Chance + boss.attack3Chance;
        float randomValue = Random.Range(0f, totalChance);

        if (randomValue < boss.attack1Chance)
            return 0; // 攻击1 - 普通攻击
        else if (randomValue < boss.attack1Chance + boss.attack2Chance)
            return 1; // 攻击2 - 中等攻击
        else
            return 2; // 攻击3 - 重击
    }

    /// <summary>
    /// 获取对应攻击的伤害值
    /// </summary>
    private int GetAttackDamage(int index)
    {
        switch (index)
        {
            case 0: return boss.attack1Damage;
            case 1: return boss.attack2Damage;
            case 2: return boss.attack3Damage;
            default: return boss.attack1Damage;
        }
    }

    public override void Update()
    {
        base.Update();

        // 攻击时保持静止
        boss.SetVelocity(0, 0);

        // 等待攻击动画结束
        if (triggerCalled)
        {
            // 检查是否应该进入疲劳状态
            if (boss.ShouldEnterTiredState())
            {
                stateMachine.ChangeState(boss.tiredState);
                return;
            }

            // 返回移动状态继续追击
            stateMachine.ChangeState(boss.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        // 攻击结束时关闭反击窗口
        boss.EnableCounterWindow(false);
    }
}
