using UnityEngine;

public class EnemyArcher_AttackState : Enemy_AttackState
{
    public EnemyArcher_AttackState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName) { }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            PerformRangedAttack();
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    private void PerformRangedAttack()
    {
        Enemy_Archer archer = enemy as Enemy_Archer;
        if (archer == null) return;

        if (archer.arrowPrefab != null && archer.firePoint != null)
        {
            GameObject arrow = GameObject.Instantiate(archer.arrowPrefab, archer.firePoint.position, Quaternion.identity);
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
                arrowScript.SetDirection(enemy.facingDir);
        }
    }
}

