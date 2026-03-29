using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    //Item effect can be triggered in different cases
    //1. when player takes damage - in PlayerStats.DecreaseHPBy()
    //2. when player hits enemy - in PlayerAnimationTrigger.AttackTrigger()
    //3. when player releases sword arcane (e.g. ice and fire sword) - in PlayerAnimationTrigger.AttackTrigger()
    //4. when player uses items (e.g. flask) - in Player.Update()
    //5. when player's magic hits enemies (e.g. Charm of God will add effects to crystal) - in CrystalSkillController.Explosion()

    public bool effectUsed { get; set; }
    public float effectLastUseTime { get; set; }
    public float effectCooldown;

    [TextArea]
    public string effectDescription;

    [TextArea]
    public string effectDescription_Chinese;

    public virtual void ExecuteEffect(Transform _spawnTransform)
    {
        //Debug.Log("Effect Executed");
    }
    
    //public virtual void ExecuteEffect_NoHitNeeded()
    //{

    //}

    public virtual void ReleaseSwordArcane()
    {

    }

}
