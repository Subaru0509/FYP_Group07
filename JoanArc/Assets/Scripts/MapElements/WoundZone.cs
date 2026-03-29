using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundZone : MapElement
{
    private bool hasDamagedPlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasDamagedPlayer)
        {
            if (collision.GetComponent<Player>() != null)
            {
                //this damage can't kill player here lmao
                int damage = 30;

                //if player's hp is below 30 rn, this damage will make player only have 1 hp left
                if (PlayerManager.instance.player.stats.currentHP < 30)
                {
                    damage = PlayerManager.instance.player.stats.currentHP - 1;
                }

                collision.GetComponent<PlayerStats>()?.TakeDamage(damage, transform, collision.transform, false);
                hasDamagedPlayer = true;

                GameManager.instance.UsedMapElementIDList.Add(mapElementID);
            }
        }
    }
}
