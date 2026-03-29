using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemy Effect", menuName = "Data/Item Effect/Freeze Enemy Effect")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float freezeDuration;

    public override void ExecuteEffect(Transform _spawnTransform)
    {
        //freeze enemy effect will only be triggered when HP is below 50%
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHP > playerStats.getMaxHP() * 0.5)
        {
            return;
        }


        Collider2D[] colliders = Physics2D.OverlapCircleAll(_spawnTransform.position, 2);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                enemy.FreezeEnemyForTime(freezeDuration);
            }
        }
    }
}
