using UnityEngine;

public class PlayerDownStrikeState : PlayerState
{
    public bool fallingStrikeTrigger { get; private set; } = false;
    public bool animStopTrigger { get; private set; } = false;

    private bool animStopTriggerHasBeenSet = false;
    private bool screenShakeHasBeenSet = false;
    public PlayerDownStrikeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, -10);

        fallingStrikeTrigger = false;
        animStopTrigger = false;
        animStopTriggerHasBeenSet = false;
        screenShakeHasBeenSet = false;

        stateTimer = Mathf.Infinity;
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.speed = 1;
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != this)
        {
            return;
        }

        //player will first rise up a bit as the falling strike "charge" stage
        if (!fallingStrikeTrigger)
        {
            player.SetVelocity(0, 1.2f);
        }
        else
        {
            //player will strike down
            player.SetVelocity(0, -17);

            //when the sword is fully reached out to attack enemy, anim will stop at this stage
            if (animStopTrigger && !animStopTriggerHasBeenSet)
            {
                player.anim.speed = 0;
                animStopTriggerHasBeenSet = true;
            }

            //when player reaches ground, anim will continue playing
            if (player.IsGroundDetected())
            {
                player.anim.speed = 1;

                if (!screenShakeHasBeenSet)
                {
                    player.fx.ScreenShake(player.fx.shakeDirection_medium);
                    player.fx.PlayDownStrikeDustFX();
                    screenShakeHasBeenSet = true;
                }
            }

            //if triggerCalled has been set to true, player's down strike is finished
            if (triggerCalled)
            {
                stateMachine.ChangeState(player.idleState);
            }

        }
    }

    public void SetFallingStrikeTrigger()
    {
        fallingStrikeTrigger = true;
    }

    public void SetAnimStopTrigger()
    {
        animStopTrigger = true;
    }

}