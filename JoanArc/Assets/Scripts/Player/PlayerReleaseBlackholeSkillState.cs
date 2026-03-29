using UnityEngine;

public class PlayerReleaseBlackholeSkillState : PlayerState
{
    private float flyTime = 0.4f;
    private bool skillUsed;
    private float originalGravity;

    public PlayerReleaseBlackholeSkillState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        originalGravity = rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("Player has exited blackhole skill state");
        rb.gravityScale = originalGravity;
        player.fx.MakeEntityTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != player.blackholeSkillState)
        {
            return;
        }

        if (Camera.main.orthographicSize <= 10)
        {
            Camera.main.orthographicSize += 0.1f;
        }

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!skillUsed)
            {
                if (player.skill.blackhole.UseSkillIfAvailable())
                {
                    skillUsed = true;
                }
            }

            if (player.skill.blackhole.CanExitBlackholeSkill())
            {
                player.stateMachine.ChangeState(player.airState);
            }
        }
    }
}
