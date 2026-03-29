using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_ThunderStrikeController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyStats, transform);
        }
    }

}
