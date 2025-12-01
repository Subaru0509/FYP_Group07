using UnityEngine;

public class EnemyArcher_BattleState : Enemy_BattleState
{
    public EnemyArcher_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Update()
    {
        base.Update();

        float distance = DistanceToPlayer();

        if (distance < enemy.minRetreatDistance)
        {
            // Player is too close ¡ú step back
            enemy.SetVelocity(-enemy.battleMoveSpeed * enemy.facingDir, rb.velocity.y);
        }
        else if (distance > enemy.attackDistance)
        {
            // The player is too far away ¡ú Move forward
            enemy.SetVelocity(enemy.battleMoveSpeed * enemy.facingDir, rb.velocity.y);
        }
        else
        {
            // When the player is within the appropriate range ¡ú stop moving and enter the attack state
            enemy.SetVelocity(0, rb.velocity.y);
            stateMachine.ChangeState(enemy.attackState);
        }
    }

    /// <summary>
    /// Independently implemented player distance calculation method
    /// </summary>
    private float DistanceToPlayer()
    {
        Transform player = enemy.GetPlayerReference();
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
}


