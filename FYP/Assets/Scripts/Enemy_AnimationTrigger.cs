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
        if (enemyVfx != null)
            enemyVfx.EnableAttackAlert(true);
        if (enemy != null)
            enemy.EnableCounterWindow(true);
    }

    private void DisableCounterWindow()
    {
        if (enemyVfx != null)
            enemyVfx.EnableAttackAlert(false);
        if (enemy != null)
            enemy.EnableCounterWindow(false);
    }
}
