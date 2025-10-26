using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadStste : EnemyState
{
    public Enemy_DeadStste(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(animBoolName, true);

        stateMachine.SwitchCanChangeState();

        //enemy.StartCoroutine(DisappearAfterSeconds(5f));
    }

    //private IEnumerator DisappearAfterSeconds(float seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    GameObject.Destroy(enemy.gameObject);
    //}
}
