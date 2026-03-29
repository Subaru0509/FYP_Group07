using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class Buff_Effect : ItemEffect
{
    [SerializeField] private StatType buffStatType;
    [SerializeField] private int buffValue;
    [SerializeField] private int buffDuration;

    private PlayerStats playerStats;

    //public override void ExecuteEffect_NoHitNeeded()
    //{
    //    playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

    //    playerStats.IncreaseStatByTime(GetBuffStat(buffStatType), buffValue, buffDuration);
    //}

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.IncreaseStatByTime(playerStats.GetStatByType(buffStatType), buffValue, buffDuration);
    }



}
