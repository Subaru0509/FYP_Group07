using UnityEngine;

public class Player_BlockState : Player_GroundedState
{
    private float blockDuration = 1f;
    private float blockTimer;
    

    public Player_BlockState(Player player, StateMachine stateMachine, string amimBoolName) : base(player, stateMachine, amimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        blockTimer = blockDuration;
    }

    public override void Update()
    {
        base.Update();


        blockTimer -= Time.deltaTime;
        player.SetVelocity(0, rb.velocity.y);

        if (!input.Player.Block.IsPressed() || blockTimer <= 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

      
        
    }
}
