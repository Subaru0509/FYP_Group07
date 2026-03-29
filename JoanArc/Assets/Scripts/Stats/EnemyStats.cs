using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop itemDropSystem;

    public Stat currencyDropAmount;

    [Header("Enemy Level Info")]
    [SerializeField] private int enemyLevel = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f;

    protected override void Start()
    {
        //currencyDropAmount.SetDefaultValue(100);

        ModifyAllStatsAccordingToEnemyLevel();

        base.Start();

        enemy = GetComponent<Enemy>();
        itemDropSystem = GetComponent<ItemDrop>();

    }


    public override void TakeDamage(int _damage, Transform _attacker, Transform _attackee, bool _isCrit)
    {
        base.TakeDamage(_damage, _attacker, _attackee, _isCrit);

        //this will interupt skeleton's action even if enemy is attacking player!
        //because in skeleton.GetintoBattleState() it doesn't check if current state is attack state
        //the original purpose of this is to make enemy enter battle state immediately
        //if player is attacking enemy from behind
        enemy.GetIntoBattleState();
    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();

        //enemy will drop items when dying
        itemDropSystem.GenrateDrop();

        //player will get currency when killing enemy
        PlayerManager.instance.currency += currencyDropAmount.GetValue();

        Destroy(gameObject, 3f);
    }

    public void ZeroHP()
    {
        currentHP = 0;

        base.Die();

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public void DropCurrencyAndItem()
    {
        itemDropSystem.GenrateDrop();

        //player will get currency when killing enemy
        PlayerManager.instance.currency += currencyDropAmount.GetValue();
    }

    private void ModifyAllStatsAccordingToEnemyLevel()
    {
        ModifyStatAccordingToEnemyLevel(strength);
        ModifyStatAccordingToEnemyLevel(agility);
        ModifyStatAccordingToEnemyLevel(intelligence);
        ModifyStatAccordingToEnemyLevel(vitality);

        ModifyStatAccordingToEnemyLevel(damage);
        ModifyStatAccordingToEnemyLevel(critChance);
        ModifyStatAccordingToEnemyLevel(critPower);

        ModifyStatAccordingToEnemyLevel(maxHP);
        ModifyStatAccordingToEnemyLevel(armor);
        ModifyStatAccordingToEnemyLevel(evasion);
        ModifyStatAccordingToEnemyLevel(magicResistance);

        ModifyStatAccordingToEnemyLevel(fireDamage);
        ModifyStatAccordingToEnemyLevel(iceDamage);
        ModifyStatAccordingToEnemyLevel(lightningDamage);

        ModifyStatAccordingToEnemyLevel(currencyDropAmount);
    }

    private void ModifyStatAccordingToEnemyLevel(Stat _stat)
    {
        //when enemyLevel > 1, increase enemy's stats
        for (int i = 1; i < enemyLevel; i++)
        {
            float _modifier = _stat.GetValue() * percentageModifier;
            _stat.AddModifier(Mathf.RoundToInt(_modifier));
        }
    }
}
