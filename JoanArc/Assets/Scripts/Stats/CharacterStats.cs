using System.Collections;
using TMPro;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHP,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;  //damage + 1; crit_power + 1%
    public Stat agility;  //evasion + 1%; crit_chance + 1%
    public Stat intelligence; //magic_damage + 1; magic_resistance + 3
    public Stat vitality; //maxHP + 5

    [Header("Defensive Stats")]
    public Stat maxHP;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;  //critPower = 150% by default

    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [Header("Ailments Info")]
    public bool isIgnited; //does damage over time
    public bool isChilled; //armor - 20%
    public bool isShocked; //accuracy - 20% (enemy evasion + 20%)
    public Stat igniteDuration;
    public Stat chillDuration;
    public Stat shockDuration;
    [SerializeField] private GameObject thunderStrikePrefab;
    private int thunderStrikeDamage;

    //invicible detection is in DoDamage(), there's no invincible detection in TakeDamage()
    public bool isInvincible { get; private set; }

    [Space]
    public int currentHP;

    //see ApplyAilment()
    private float ignitedAilmentTimer;
    private float chilledAilmentTimer;
    private float shockedAilmentTimer;

    private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int igniteDamage; //is set up in DoMagicDamage()

    //vulnerable state will take 10% more damage
    public bool isVulnerable { get; private set; }
    public bool isDead { get; private set; }

    //Stats calculation:
    //total_evasion = evasion + agility + [attacker shock effect];
    //total_damage = (damage + strength) * [total_crit_power] - target.armor * [target chill effect];
    //total_crit_chance = critChance + agility;
    //total_crit_power = critPower + strength;
    //total_magic_damage = (fireDamage + iceDamage + lightningDamage + intelligence) - (target.magicResistance + 3 * target.intelligence);

    public System.Action onHealthChanged;

    //To make entity HP bar correctly updated in start
    //if not willing to use this bool variable
    //can change script execution order in unity project settings
    //order: CharacterStats -> HPBar_UI
    //[HideInInspector] public bool HPBarCanBeInitialized;

    protected EntityFX fx;

    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {
        currentHP = getMaxHP();
        critPower.SetDefaultValue(150);

        //HPBarCanBeInitialized = true;
    }

    protected virtual void Update()
    {
        ignitedAilmentTimer -= Time.deltaTime;
        chilledAilmentTimer -= Time.deltaTime;
        shockedAilmentTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedAilmentTimer < 0)
        {
            isIgnited = false;
        }

        if (chilledAilmentTimer < 0)
        {
            isChilled = false;
        }

        if (shockedAilmentTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            DealIgniteDamage();
        }

    }



    public virtual void DoDamge(CharacterStats _targetStats)
    {
        //if target is invincible, it won't take damage
        if (_targetStats.isInvincible)
        {
            return;
        }

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

        _totalDamage = CheckTargetArmor(_targetStats, _totalDamage);

        _totalDamage = CheckTargetVulnerability(_targetStats, _totalDamage);

        _targetStats.TakeDamage(_totalDamage, transform, _targetStats.transform, crit);

        //DoMagicDamage(_targetStats, transform);
    }



    public virtual void TakeDamage(int _damage, Transform _attacker, Transform _attackee, bool _isCrit)
    {
        //if entity is invincible, it won't take damage
        if (isInvincible)
        {
            return;
        }

        DecreaseHPBy(_damage, _isCrit);

        //Debug.Log($"{gameObject.name} received {_damage} damage");

        _attackee.GetComponent<Entity>()?.DamageFlashEffect();
        _attackee.GetComponent<Entity>()?.DamageKnockbackEffect(_attacker, _attackee);

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        Debug.Log($"{gameObject.name} is Dead");
    }

    public virtual void DieFromFalling()
    {
        if (isDead)
        {
            return;
        }

        currentHP = 0;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }

        Die();
    }

    public void BecomeInvincible(bool _invincible)
    {
        isInvincible = _invincible;
    }




    #region Magic and Ailments
    public virtual void DoMagicDamage(CharacterStats _targetStats, Transform _attacker)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int _totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        _totalMagicDamage = CheckTargetMagicResistance(_targetStats, _totalMagicDamage);

        _targetStats.TakeDamage(_totalMagicDamage, _attacker, _targetStats.transform, false);

        //only if at least 1 of the magic damage is > 0, ailment can be applied
        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        //choose the highest magic damage to apply the related ailment
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        //if all the magic damage are the same, randomly pick 1 ailment to apply
        if (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.3f && _fireDamage > 0)
            {
                canApplyIgnite = true;

                if (canApplyIgnite)
                {
                    _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
                }

                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < 0.5f && _lightningDamage > 0)
            {
                canApplyShock = true;

                if (canApplyShock)
                {
                    _targetStats.SetupThunderStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.2f));
                }

                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        //set up ignite damage
        //ignite_damage = fire_damage * 0.2
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupThunderStrikeDamage(Mathf.RoundToInt(_lightningDamage * 0.2f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedAilmentTimer = igniteDuration.GetValue();

            //fx.EnableIgniteFXForTime(ignitedAilmentTimer);
            StartCoroutine(fx.EnableIgniteFXForTime_Coroutine(ignitedAilmentTimer));
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledAilmentTimer = chillDuration.GetValue();

            //fx.EnableChillFXForTime(chilledAilmentTimer);
            StartCoroutine(fx.EnableChillFXForTime_Coroutine(chilledAilmentTimer));

            float _slowPercentage = 0.2f;
            GetComponent<Entity>()?.SlowSpeedBy(_slowPercentage, chillDuration.GetValue());
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShockAilment(_shock);
            }
            else //when attacking on shocked enemies, thunder strike will be generated and hit enemies
            {
                //prevent cases where enemies' attacks on player
                //can still generate thunder strike
                //and hit enemeies themselves
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                GenerateThunderStrikeAndHitClosestEnemy(7.5f);

            }
        }

        if (isIgnited)
        {
            Debug.Log($"{gameObject.name} is Ignited");
        }
        else if (isChilled)
        {
            Debug.Log($"{gameObject.name} is Chilled");
        }
        else if (isShocked)
        {
            Debug.Log($"{gameObject.name} is Shocked");
        }
    }

    public void ApplyShockAilment(bool _shock)
    {
        //can't apply shock ailment on enemy who's already shocked
        if (isShocked)
        {
            return;
        }

        isShocked = _shock;
        shockedAilmentTimer = shockDuration.GetValue();

        //fx.EnableShockFXForTime(shockedAilmentTimer);
        StartCoroutine(fx.EnableShockFXForTime_Coroutine(shockedAilmentTimer));
    }

    private void GenerateThunderStrikeAndHitClosestEnemy(float _targetScanRadius)
    {
        //find closest target
        //instantiate thunder strike
        //setup thunder strike
        Transform closestEnemy = null;

        //find all the enemies inside the search radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _targetScanRadius);

        float closestDistanceToEnemy = Mathf.Infinity;

        //find closest enemy
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float currentDistanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (currentDistanceToEnemy < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = currentDistanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        //if can't find closestEnemy,
        //the enemy himself is just the closest enemy
        if (closestEnemy == null)
        {
            closestEnemy = transform;
        }

        //generate thunder strike and hit closest enemy
        if (closestEnemy != null)
        {
            GameObject newThunderStrike = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);

            newThunderStrike.GetComponent<Skill_ThunderStrikeController>()?.Setup(thunderStrikeDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    private void DealIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            Debug.Log($"Take burn damage {igniteDamage}");
            DecreaseHPBy(igniteDamage, false);

            if (currentHP <= 0 && !isDead)
            {
                Die();
            }

            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }

    public void SetupIgniteDamage(int _igniteDamage)
    {
        igniteDamage = _igniteDamage;
    }

    public void SetupThunderStrikeDamage(int _thunderStrikeDamage)
    {
        thunderStrikeDamage = _thunderStrikeDamage;
    }
    #endregion



    #region HP and Damage Calculation - Armor, Crit, Magic Resistance, Vulnerability, Evasion
    protected int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        //chill effect: reduce armor by 20%
        if (_targetStats.isChilled)
        {
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            _totalDamage -= _targetStats.armor.GetValue();
        }


        //make totalDamge >= 0
        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }

    private int CheckTargetMagicResistance(CharacterStats _targetStats, int _totalMagicDamage)
    {
        _totalMagicDamage -= _targetStats.magicResistance.GetValue() + (3 * _targetStats.intelligence.GetValue());

        _totalMagicDamage = Mathf.Clamp(_totalMagicDamage, 0, int.MaxValue);
        return _totalMagicDamage;
    }

    protected bool TargetCanEvadeThisAttack(CharacterStats _targetStats)
    {
        int _totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            _totalEvasion += 20;
        }

        if (Random.Range(0, 100) < _totalEvasion)
        {
            //Debug.Log("Attack Evaded");
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int _totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= _totalCritChance)
        {
            return true;
        }

        return false;
    }

    protected int CalculatCritDamage(int _damage)
    {
        float _totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * _totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int CheckTargetVulnerability(CharacterStats _targetStats, int _totalDamage)
    {
        if (_targetStats.isVulnerable)
        {
            _totalDamage = Mathf.RoundToInt(_totalDamage * 1.1f);
        }

        return _totalDamage;
    }

    public virtual void OnEvasion()
    {

    }


    #region HP
    public virtual int DecreaseHPBy(int _takenDamage, bool _isCrit)
    {
        currentHP -= _takenDamage;

        if (_takenDamage > 0)
        {
            GameObject popUpText = fx.CreatePopUpText(_takenDamage.ToString());

            if (_isCrit)
            {
                popUpText.GetComponent<TextMeshPro>().color = Color.yellow;
                popUpText.GetComponent<TextMeshPro>().fontSize = 12;
            }
        }

        Debug.Log($"{gameObject.name} takes {_takenDamage} damage");

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }

        return _takenDamage;
    }

    public virtual void IncreaseHPBy(int _HP)
    {
        currentHP += _HP;

        if (currentHP > getMaxHP())
        {
            currentHP = getMaxHP();
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
    #endregion

    #endregion

    public void BecomeVulnerableForTime(float _seconds)
    {
        StartCoroutine(BecomeVulnerableForTime_Coroutine(_seconds));
    }

    private IEnumerator BecomeVulnerableForTime_Coroutine(float _duration)
    {
        isVulnerable = true;
        //Debug.Log("Vulnerable!");
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
        //Debug.Log("Exit Vulnerable!");
    }

    public virtual void IncreaseStatByTime(Stat _statToModify, int _modifier, float _duration)
    {
        StartCoroutine(StatModify_Coroutine(_statToModify, _modifier, _duration));
    }

    private IEnumerator StatModify_Coroutine(Stat _statToModify, int _modifier, float _duration)
    {
        _statToModify.AddModifier(_modifier);
        Inventory.instance.UpdateStatUI();

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
        Inventory.instance.UpdateStatUI();
    }

    public Stat GetStatByType(StatType _statType)
    {
        Stat stat = null;

        switch (_statType)
        {
            case StatType.strength: stat = strength; break;
            case StatType.agility: stat = agility; break;
            case StatType.intelligence: stat = intelligence; break;
            case StatType.vitality: stat = vitality; break;
            case StatType.damage: stat = damage; break;
            case StatType.critChance: stat = critChance; break;
            case StatType.critPower: stat = critPower; break;
            case StatType.maxHP: stat = maxHP; break;
            case StatType.armor: stat = armor; break;
            case StatType.evasion: stat = evasion; break;
            case StatType.magicResistance: stat = magicResistance; break;
            case StatType.fireDamage: stat = fireDamage; break;
            case StatType.iceDamage: stat = iceDamage; break;
            case StatType.lightningDamage: stat = lightningDamage; break;
        }

        return stat;
    }


    #region Get Final Stat Value
    public int getMaxHP()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }

    public int GetDamage()
    {
        return damage.GetValue() + strength.GetValue();
    }

    public int GetCritPower()
    {
        return critPower.GetValue() + strength.GetValue();
    }

    public int GetCritChance()
    {
        return critChance.GetValue() + agility.GetValue();
    }

    public int GetEvasion()
    {
        return evasion.GetValue() + agility.GetValue();
    }

    public int GetMagicResistance()
    {
        return magicResistance.GetValue() + intelligence.GetValue() * 3;
    }

    public int GetFireDamage()
    {
        return fireDamage.GetValue() + intelligence.GetValue();
    }

    public int GetIceDamage()
    {
        return iceDamage.GetValue() + intelligence.GetValue();
    }

    public int GetLightningDamage()
    {
        return lightningDamage.GetValue() + intelligence.GetValue();
    }
    #endregion
}
