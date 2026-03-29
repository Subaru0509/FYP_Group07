using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    protected Player player;

    public float cooldown;
    protected float cooldownTimer;
    public float skillLastUseTime { get; protected set; } = 0;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        CheckUnlockFromSave();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlockFromSave()
    {

    }

    public virtual bool SkillIsReadyToUse()
    {
        if (cooldownTimer < 0)
        {
            return true;
        }
        else
        {
            //english
            if (LanguageManager.instance.localeID == 0)
            {
                player.fx.CreatePopUpText("Skill is in cooldown");

            }
            //chinese
            else if (LanguageManager.instance.localeID == 1)
            {
                player.fx.CreatePopUpText("技能冷却中！");
            }
            return false;
        }
    }

    public virtual bool UseSkillIfAvailable()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        //english
        if (LanguageManager.instance.localeID == 0)
        {
            player.fx.CreatePopUpText("Skill is in cooldown");

        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            player.fx.CreatePopUpText("技能冷却中！");
        }
        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindClosestEnemy(Transform _searchCenter)
    {
        Transform closestEnemy = null;

        //find all the enemies inside the search radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_searchCenter.position, 12);

        float closestDistanceToEnemy = Mathf.Infinity;

        //find closest enemy
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float currentDistanceToEnemy = Vector2.Distance(_searchCenter.position, hit.transform.position);

                if (currentDistanceToEnemy < closestDistanceToEnemy)
                {
                    closestDistanceToEnemy = currentDistanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

    protected virtual Transform ChooseRandomEnemy(Transform _searchCenter, float _targetSearchRadius)
    {
        Transform targetEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _targetSearchRadius);

        //Find enemies inside the searchRadius
        List<Transform> enemies = new List<Transform>();

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                enemies.Add(hit.transform);
            }
        }

        //if successfully finds enemies around,
        //randomly select one to be targetEnemy
        if (enemies.Count > 0)
        {
            targetEnemy = enemies[Random.Range(0, enemies.Count)];
        }

        return targetEnemy;
    }
}
