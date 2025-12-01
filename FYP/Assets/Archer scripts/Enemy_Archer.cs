using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Archer Settings")]
    public GameObject arrowPrefab;   // Drag the arrow prefab in the Inspector
    public Transform firePoint;      // Drag the launch point (usually the front position of the bow) in the Inspector

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new EnemyArcher_AttackState(this, stateMachine, "attack");
        battleState = new EnemyArcher_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadStste(this, stateMachine, "dead");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }
}
