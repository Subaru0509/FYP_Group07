using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    protected Rigidbody2D rb;
    protected Animator anim;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;


    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        enemyBase = _enemyBase;
        stateMachine = _stateMachine;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;

        rb = enemyBase.rb;
        anim = enemyBase.anim;

        anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);

        enemyBase.AssignLastAnimBoolName(animBoolName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
