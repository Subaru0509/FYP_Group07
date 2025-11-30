using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTrigger : Entity_AnimationTriggers
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }
    private void EnableCounterWindow()
    {
        enemy.EnableCounterWindow(true);
    }

    private void DisableCounterWindow()
    {
        enemy.EnableCounterWindow(false);
    }
}
