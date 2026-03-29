using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownStrikeAttackCollider : MonoBehaviour
{
    private Player player;

    private float downStrikAttackCooldown = 10f;
    private float downStrikeAttackTimer;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        downStrikeAttackTimer -= Time.deltaTime;

        if (player.IsGroundDetected())
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        downStrikeAttackTimer = 0;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (player.stateMachine.currentState == player.downStrikeState)
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                Vector2 originalEnemyKnockbackMovement = enemy.knockbackMovement;
                enemy.knockbackMovement = new Vector2(0, player.rb.velocity.y);

                enemy.SetVelocity(player.rb.velocity.x, player.rb.velocity.y);

                if (downStrikeAttackTimer < 0)
                {
                    player.stats.DoDamge(enemy.stats);
                    downStrikeAttackTimer = downStrikAttackCooldown;
                }

                enemy.knockbackMovement = originalEnemyKnockbackMovement;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player.stateMachine.currentState == player.downStrikeState)
        {
            if (collision.GetComponent<Enemy>() != null)
            {
                Enemy enemy = collision.GetComponent<Enemy>();

                //Vector2 originalEnemyKnockbackMovement = enemy.knockbackMovement;
                //enemy.knockbackMovement = new Vector2(0, player.rb.velocity.y);

                enemy.SetVelocity(player.rb.velocity.x, player.rb.velocity.y);
                player.stats.DoDamge(enemy.stats);

                //enemy.knockbackMovement = originalEnemyKnockbackMovement;
            }
        }
    }

}
