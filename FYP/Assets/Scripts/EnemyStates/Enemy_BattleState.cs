using UnityEngine;

public class Enemy_BattleState : EnemyState
{

    private Transform player;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();           
        
        UpdateBattleTimer();
        
        player ??= enemy.GetPlayerReference();

        if (shouldRetreat())
        {
            rb.velocity = new Vector2(enemy.retreaVelocity.x * -DirectionToPlayer(), enemy.retreaVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }

    }

    public override void Update()
    {
        base.Update();


        if (enemy.PlayerDetected())
            UpdateBattleTimer();

        if (BattleTimeIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if(enemy.wallDectected)
            stateMachine.ChangeState(enemy.idleState);

        if (WithAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.velocity.y);


    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool BattleTimeIsOver() => Time.time >= lastTimeWasInBattle + enemy.battleTimeDuration;

    private bool WithAttackRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool shouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;


    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
        
    }

    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
