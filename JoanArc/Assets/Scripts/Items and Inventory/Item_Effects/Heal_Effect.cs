using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Effect/Heal Effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent; // Healing percentage

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healHP = Mathf.RoundToInt(playerStats.getMaxHP() * healPercent); // Amount of healing
        playerStats.IncreaseHPBy(healHP);
    }
}
