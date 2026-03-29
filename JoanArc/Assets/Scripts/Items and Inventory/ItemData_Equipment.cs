using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Charm,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique Item Effect info")]
    //public float itemCooldown;
    //public bool itemUsed { get; set; }
    //public float itemLastUseTime { get; set; }
    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength;  //damage + 1; crit_power + 1%
    public int agility;  //evasion + 1%; crit_chance + 1%
    public int intelligence; //magic_damage + 1; magic_resistance + 3
    public int vitaliy; //maxHP + 5

    [Header("Defensive Stats")]
    public int maxHP;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;  //critPower = 150% by default

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    [Header("Craft Requirements")]
    public List<InventorySlot> requiredCraftMaterials;

    private int statInfoLength;
    //private int minStatInfoLength = 5;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitaliy);

        playerStats.maxHP.AddModifier(maxHP);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitaliy);

        playerStats.maxHP.RemoveModifier(maxHP);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    //will be triggerd in scripts like animationTrigger when attacking enemies
    //private void ExecuteItemEffect(Transform _spawnTransform)
    //{
    //    foreach (var effect in itemEffects)
    //    {
    //        effect.ExecuteEffect(_spawnTransform);
    //    }
    //}

    //public void ExecuteItemEffect_NoHitNeeded()
    //{
    //    foreach (var effect in itemEffects)
    //    {
    //        effect.ExecuteEffect_NoHitNeeded();
    //    }
    //}

    //private void ReleaseSwordArcane()
    //{
    //    foreach (var effect in itemEffects)
    //    {
    //        effect.ReleaseSwordArcane();
    //    }
    //}

    public void RefreshUseState()
    {
        //itemUsed = false;
        //itemLastUseTime = 0;

        foreach (var effect in itemEffects)
        {
            effect.effectUsed = false;
            effect.effectLastUseTime = 0;
        }
    }

    public void ExecuteItemEffect_ConsiderCooldown(Transform _spawnTransform)
    {
        // >= here to prevent the case
        // where mutilple 0-cooldown effects need to get executed at the same time
        // but all of the effects next to the first one will be in cooldown

        foreach (var effect in itemEffects)
        {
            bool canUseEffect = Time.time >= effect.effectLastUseTime + effect.effectCooldown;

            if (canUseEffect || !effect.effectUsed)
            {
                //ExecuteItemEffect(_spawnTransform);
                effect.ExecuteEffect(_spawnTransform);
                effect.effectLastUseTime = Time.time;
                effect.effectUsed = true;
                Inventory.instance.UpdateStatUI();
                Debug.Log($"Use Item Effect: {effect.name}");
            }
            else
            {
                Debug.Log("Item Effect is in cooldown");
            }
        }
    }

    public void ReleaseSwordArcane_ConsiderCooldown()
    {
        // >= here to prevent the case
        // where mutilple 0-cooldown effects need to get executed at the same time
        // but all of the effects next to the first one will be in cooldown

        foreach (var effect in itemEffects)
        {
            bool canUseEffect = Time.time >= effect.effectLastUseTime + effect.effectCooldown;

            if (canUseEffect || !effect.effectUsed)
            {
                effect.ReleaseSwordArcane();
                effect.effectLastUseTime = Time.time;
                effect.effectUsed = true;
                Inventory.instance.UpdateStatUI();
                Debug.Log($"Use Sword Arcane: {effect.name}");
            }
            else
            {
                Debug.Log("Item Effect is in cooldown");
            }
        }
    }

    public override string GetItemStatInfoAndEffectDescription()
    {
        sb.Length = 0;
        statInfoLength = 0;

        AddItemStatInfo(strength, "Strength");
        AddItemStatInfo(agility, "Agility");
        AddItemStatInfo(intelligence, "Intelligence");
        AddItemStatInfo(vitaliy, "Vitality");

        AddItemStatInfo(damage, "Damage");
        AddItemStatInfo(critChance, "Crit Chance");
        AddItemStatInfo(critPower, "Crit Power");

        AddItemStatInfo(maxHP, "Max HP");
        AddItemStatInfo(evasion, "Evasion");
        AddItemStatInfo(armor, "Armor");
        AddItemStatInfo(magicResistance, "Magic Resist");

        AddItemStatInfo(fireDamage, "Fire Dmg");
        AddItemStatInfo(iceDamage, "Ice Dmg");
        AddItemStatInfo(lightningDamage, "Lightning Dmg");

        if (itemEffects.Length > 0 && statInfoLength > 0)
        {
            if (itemEffects[0].effectDescription.Length > 0)
            {
                //add space between stat info and effect description;
                sb.AppendLine();
            }
        }

        for (int i = 0; i < itemEffects.Length; i++)
        {
            sb.AppendLine();

            //english
            if (LanguageManager.instance.localeID == 0)
            {
                if (itemEffects[i].effectDescription.Length > 0)
                {
                    sb.Append($"[unique effect]\n{itemEffects[i].effectDescription}\n");
                }
            }
            //chinese
            else if (LanguageManager.instance.localeID == 1)
            {
                if (itemEffects[i].effectDescription_Chinese.Length > 0)
                {
                    sb.Append($"[¹ÌÓÐÐ§¹û]\n{itemEffects[i].effectDescription_Chinese}\n");
                }
            }

            statInfoLength++;
        }

        //make sure the space below unique effect is same as the one below stat info so that item tool tip looks nicer
        if (sb.ToString()[sb.Length - 1] == '\n')
        {
            sb.Remove(sb.Length - 1, 1);
        }


        //if (statInfoLength < minStatInfoLength)
        //{
        //    int _numberOfLinesToApped = minStatInfoLength - statInfoLength;
        //    //StringBuilder.Append() will auto add a line
        //    //if the String is empty
        //    //so here make numberOfLinesToAppend-- to prevent adding an extra line
        //    if (statInfoLength == 0)
        //    {
        //        _numberOfLinesToApped--;
        //    }

        //    for (int i = 0; i < _numberOfLinesToApped; i++)
        //    {
        //        sb.AppendLine();
        //        sb.Append("");
        //    }
        //}

        sb.AppendLine();
        sb.Append("");
        sb.AppendLine();
        sb.Append("");

        return sb.ToString();
    }

    private void AddItemStatInfo(int _statValue, string _statName)
    {
        if (_statValue != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (_statValue > 0)
            {
                sb.Append($"+ {_statValue} {_statName}");
                statInfoLength++;
            }
        }
    }
}
