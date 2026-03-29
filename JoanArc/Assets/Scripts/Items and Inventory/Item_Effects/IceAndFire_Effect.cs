using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/Ice and Fire Effect")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xFlyVelocity;

    //summon ice and fire on third primary attack if successfully hitting enemy
    //public override void ExecuteEffect_HitNeeded(Transform _spawnTransform)
    //{
    //    Player player = PlayerManager.instance.player;

    //    bool thirdAttack = player.primaryAttackState.comboCounter == 2;

    //    if (thirdAttack)
    //    {
    //        GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _spawnTransform.position, player.transform.rotation);

    //        newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xFlyVelocity * player.facingDirection, 0);

    //        Destroy(newIceAndFire, 3);
    //    }

    //}

    //summon ice and fire on third primary attack no matter third attack hits enemy or not
    //public override void ExecuteEffect_NoHitNeeded()
    //{
    //    Player player = PlayerManager.instance.player;

    //    bool thirdAttack = player.primaryAttackState.comboCounter == 2;

    //    if (thirdAttack)
    //    {
    //        GameObject newIceAndFire = Instantiate(iceAndFirePrefab, player.transform.position, player.transform.rotation);

    //        newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xFlyVelocity * player.facingDirection, 0);

    //        Destroy(newIceAndFire, 3);
    //    }

    //}


    //summon ice and fire on third primary attack no matter third attack hits enemy or not
    public override void ReleaseSwordArcane()
    {
        Player player = PlayerManager.instance.player;

        bool thirdAttack = player.primaryAttackState.comboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, player.transform.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xFlyVelocity * player.facingDirection, 0);

            Destroy(newIceAndFire, 3);
        }
    }
}
