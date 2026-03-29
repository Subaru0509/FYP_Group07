using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemyFX : EntityFX
{
    protected override void Awake()
    {
        HPBar = GetComponentInChildren<HPBar_UI>()?.gameObject;
    }

    protected override void Start()
    {
        player = PlayerManager.instance.player;
    }

    private IEnumerator FlashFX()
    {
        yield return new WaitForSeconds(0);
    }
}
