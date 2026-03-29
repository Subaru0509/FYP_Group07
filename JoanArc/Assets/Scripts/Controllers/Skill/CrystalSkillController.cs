using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExistenceTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform targetEnemy;

    private CrystalSkill crystalSkill;

    private void Start()
    {
        crystalSkill = SkillManager.instance.crystal;
    }

    private void Update()
    {
        crystalExistenceTimer -= Time.deltaTime;

        //if no avaliable enemy inside the search radius
        //crystal will not move
        if (targetEnemy == null)
        {
            canMove = false;
        }

        if (crystalExistenceTimer < 0)
        {
            EndCrystal_ExplodeIfAvailable();
        }

        //if crystal can move towards enemy
        if (canMove)
        {
            //crystal moves towards enemy
            transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, moveSpeed * Time.deltaTime);

            //crystal destroys itself when approaching enmey
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) < 1)
            {
                canMove = false;
                EndCrystal_ExplodeIfAvailable();
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(2, 2), growSpeed * Time.deltaTime);
        }
    }

    public void SetupCrystal(float _crystalExistenceDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _targetEnemy)
    {
        crystalExistenceTimer = _crystalExistenceDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        targetEnemy = _targetEnemy;
    }

    public void EndCrystal_ExplodeIfAvailable()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explosion");

            if (!crystalSkill.crystalGunUnlocked)
            {
                crystalSkill.EnterCooldown();
            }
        }
        else
        {
            crystalSelfDestroy();
        }
    }

    public void crystalSelfDestroy()
    {
        Destroy(gameObject);

        if (!crystalSkill.crystalGunUnlocked)
        {
            crystalSkill.EnterCooldown();
        }
    }

    private void Explosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                PlayerManager.instance.player.stats.DoMagicDamage(enemy.GetComponent<CharacterStats>(), transform);

                Inventory.instance.UseCharmEffect_ConsiderCooldown(enemy.transform);
                //ItemData_Equipment equippedCharm = Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Charm);

                //if (equippedCharm != null)
                //{
                //    equippedCharm.ExecuteItemEffect(enemy.transform);
                //}
            }
        }
    }


    public void SpecifyEnemyTarget(Transform _enemy)
    {
        targetEnemy = _enemy;
    }

    //public void CrystalChooseRandomEnemy(float _searchRadius)
    //{
    //    Transform originalTargetEnemy = targetEnemy;

    //    targetEnemy = SkillManager.instance.crystal.ChooseRandomEnemy(transform, _searchRadius);

    //    if (targetEnemy == null)
    //    {
    //        Debug.Log("No enemy is chosen" +
    //            "\n will choose original closest enemy");
    //        targetEnemy = originalTargetEnemy;
    //    }
    //}

}


