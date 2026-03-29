using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyattackstete : MonoBehaviour
{
    private EnemyStateMachine machine;
   // public EnemySkeleton enemy;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
         player = PlayerManager.instance.player.transform;
      //  machine = enemy.stateMachine;
    }

    // Update is called once per frame
    void Update()
    {
       // if (Vector2.Distance(player.position, enemy.transform.position) < 3)
        {

         //   machine.ChangeState(enemy.battleState);
            // skeleton.isPlayerDetected() ||
            Debug.Log("gognji");
        }

    }
}
