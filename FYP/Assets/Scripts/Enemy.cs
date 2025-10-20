using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;

    [Header("Movement details")]
    public float moveSpeed = 1.4f;
    public float idleTime = 2;
    [Range(0,2)]
    public float moveAnimSpeedMultipller = 1;
}
