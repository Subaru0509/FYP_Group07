using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTrigger : Entity_AnimationTriggers
{
    private Enemy enemy;
    private Enemy_VFX enemyVfx;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVfx = GetComponentInParent<Enemy_VFX>();
    }
    private void EnableCounterWindow()
    {
        enemyVfx?.EnableAttackAlert(true);
        enemy?.EnableCounterWindow(true);
    }

    private void DisableCounterWindow()
    {
        enemyVfx?.EnableAttackAlert(false);
        enemy?.EnableCounterWindow(false);
    }
}
