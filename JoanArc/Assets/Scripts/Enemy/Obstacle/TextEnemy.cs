using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemy : Enemy
{
    public BoxCollider2D textCollider { get; set; }

    [Header("Death Rewards")]
    public int currencyToGive;
    public List<GameObject> objectListToShow;
    public List<GameObject> prefabListToInstantiate;

    #region States
    public TextEnemyIdleState idleState { get; private set; }
    public TextEnemyDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        textCollider = GetComponent<BoxCollider2D>();

        idleState = new TextEnemyIdleState(this, stateMachine, null, this);
        deathState = new TextEnemyDeathState(this, stateMachine, null, this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();

        if (currencyToGive != 0)
        {
            PlayerManager.instance.currency += currencyToGive;
        }

        if(objectListToShow.Count > 0)
        {
            for (int i = 0; i < objectListToShow.Count; i++)
            {
                objectListToShow[i].SetActive(true);
            }
        }

        if(prefabListToInstantiate.Count > 0)
        {
            //instantiate prefab;
        }

        stateMachine.ChangeState(deathState);
    }
}
