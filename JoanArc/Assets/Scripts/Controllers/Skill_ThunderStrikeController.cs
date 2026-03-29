using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float thunderMoveSpeed;
    private int damage;

    private Animator anim;
    private bool triggered;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (targetStats == null)
        {
            return;
        }

        if (triggered)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, thunderMoveSpeed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.2f)
        {
            triggered = true;

            anim.transform.localPosition = new Vector3(0, 0.5f);
            anim.transform.localRotation = Quaternion.identity;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            //thunder will move with enemies to prevent the case
            //that thunder is too far away from the enemy
            transform.parent = targetStats.transform;

            Invoke("DamageAndSelfDestroy", 0.25f);
            anim.SetTrigger("Hit");
        }
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShockAilment(true);
        targetStats.TakeDamage(damage, transform, targetStats.transform, false);
        Destroy(gameObject, 0.4f);

    }
}
