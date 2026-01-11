using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Bush : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        deadState = new Enemy_DeadStste(this, stateMachine, "dead");
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }
}
