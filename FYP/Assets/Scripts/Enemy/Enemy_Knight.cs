using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Knight : Enemy , ICounterable
{

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadStste(this, stateMachine, "dead");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.F))
            HandleCounter();
    }
    public void HandleCounter()
    {
        if (canBeStunned == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }
}
