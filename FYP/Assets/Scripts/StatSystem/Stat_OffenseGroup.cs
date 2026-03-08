using System;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;

    // Physical damage
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    // Elemental damage
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
