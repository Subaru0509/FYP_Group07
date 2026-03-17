namespace CardBattleSystem
{
    public enum CardType
    {
        Attack,   // 攻击卡（造成伤害）
        Skill,    // 技能卡（防御、抽牌等）
        Power     // 能力卡（持续效果）
    }

    public enum CardRarity
    {
        Common,    // 普通（灰色）
        Uncommon,  // 罕见（蓝色）
        Rare,      // 稀有（金色）
        Epic       // 史诗（紫色）
    }

    public enum TargetType
    {
        SingleEnemy,  // 单个敌人
        AllEnemies,   // 所有敌人
        Self,         // 自己
        AllAllies     // 所有友军（未来多人战斗用）
    }

    public enum CardEffectType
    {
        Damage,        // 造成伤害
        Block,         // 获得格挡
        Draw,          // 抽牌
        Heal,          // 治疗
        ApplyStatus    // 施加状态（buff/debuff）
    }

    public enum StatusEffectType
    {
        Strength,     // 力量：增加攻击力
        Weak,         // 虚弱：减少攻击力
        Vulnerable,   // 易伤：受到更多伤害
        Frail,        // 脆弱：获得更少格挡
        Poison,       // 中毒：回合结束时失去生命
        Regeneration  // 再生：回合开始时恢复生命
    }
}
