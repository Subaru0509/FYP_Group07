using System.Threading;
using UnityEngine;

public class Player_BasicAttackState : EntityState
{   
    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private bool comboAttackQueued;

    private int attackDir;
    private int comboIndex = 1;
    private int comboLimit = 3;

    private const int FirstComboIndex = 1;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            Debug.Log("Warning: Combo limit and attack velocity array length do not match");
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();

        attackDir = player.moveInput.x !=0 ? ((int)player.moveInput.x) : player.facingDir;

        anim.SetInteger("basicAttackIndex", comboIndex);

        ApplyAttackVelocity();
    }


    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPerformedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();
        
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();

        }

        else
            stateMachine.ChangeState(player.idleState);
    }

    private void QueueNextAttack()
    {
        if(comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if(attackVelocityTimer < 0)
            player.SetVelocity(0,rb.velocity.y);    
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
    private void ResetComboIndexIfNeeded()
    {

        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = FirstComboIndex;
    }

}
