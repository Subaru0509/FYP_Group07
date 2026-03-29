using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    private PlayerFX playerFX;

    private float playerDefaultMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        playerFX = GetComponent<PlayerFX>();
    }

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
        playerDefaultMoveSpeed = player.moveSpeed;
    }

    public override void DoDamge(CharacterStats _targetStats)
    {
        base.DoDamge(_targetStats);

        if (_targetStats.GetComponent<Enemy>() != null)
        {
            Player player = PlayerManager.instance.player;
            int playerComboCounter = player.primaryAttackState.comboCounter;

            if (player.stateMachine.currentState == player.primaryAttackState)
            {
                if (playerComboCounter <= 1)
                {
                    playerFX.ScreenShake(playerFX.shakeDirection_light);
                }
                else
                {
                    playerFX.ScreenShake(playerFX.shakeDirection_medium);
                }
            }
            //else //for throw sword
            //{
            //    fx.ScreenShake(fx.shakeDirection_medium);
            //}

        }
    }

    public override void TakeDamage(int _damage, Transform _attacker, Transform _attackee, bool _isCrit)
    {
        //if (isInvincible)
        //{
        //    return;
        //}

        int takenDamage = DecreaseHPBy(_damage, _isCrit);

        //Debug.Log($"{gameObject.name} received {_damage} damage");

        _attackee.GetComponent<Entity>()?.DamageFlashEffect();

        SlowerPlayerMoveSpeedForTime(0.2f);

        //player will get knockbacked when the taken damage is bigger than 30% of maxHP
        if (takenDamage >= player.stats.getMaxHP() * 0.3f)
        {
            _attackee.GetComponent<Entity>()?.DamageKnockbackEffect(_attacker, _attackee);
            player.fx.ScreenShake(player.fx.shakeDirection_heavy);
        }

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    private void SlowerPlayerMoveSpeedForTime(float _duration)
    {
        float defaultMoveSpeed = player.moveSpeed;

        //im lazy here so i just use duration to slower player move speed
        player.moveSpeed = player.moveSpeed * _duration;

        Invoke("ReturnToDefaultMoveSpeed", _duration);
    }

    private void ReturnToDefaultMoveSpeed()
    {
        player.moveSpeed = playerDefaultMoveSpeed;
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.droppedCurrencyAmount = PlayerManager.instance.GetCurrentCurrency();
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenrateDrop();
    }

    public override int DecreaseHPBy(int _takenDamage, bool _isCrit)
    {
        base.DecreaseHPBy(_takenDamage, _isCrit);

        int randomIndex = Random.Range(34, 36);
        AudioManager.instance.PlaySFX(randomIndex, player.transform);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Armor);

        if (currentArmor != null)
        {
            //currentArmor.ExecuteItemEffect(player.transform);
            Inventory.instance.UseArmorEffect_ConsiderCooldown(player.transform);
        }

        return _takenDamage;
    }

    public override void OnEvasion()
    {
        Debug.Log("Player evaded attack!");
        player.skill.dodge.CreateMirageOnDodge();
    }

    public void CloneDoDamage(CharacterStats _targetStats, float _cloneAttackDamageMultipler, Transform _cloneTransform)
    {
        if (TargetCanEvadeThisAttack(_targetStats))
        {
            return;
        }

        int _totalDamage = damage.GetValue() + strength.GetValue();

        bool crit = CanCrit();

        if (crit)
        {
            Debug.Log("Critical Attack!");
            _totalDamage = CalculatCritDamage(_totalDamage);
        }

        fx.CreateHitFX(_targetStats.transform, crit);

        //clone attack damage should be less than player's damage
        if (_cloneAttackDamageMultipler > 0)
        {
            _totalDamage = Mathf.RoundToInt(_totalDamage * _cloneAttackDamageMultipler);
        }

        _totalDamage = CheckTargetArmor(_targetStats, _totalDamage);
        _totalDamage = CheckTargetVulnerability(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, _cloneTransform, _targetStats.transform, crit);
    }
}
