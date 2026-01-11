using UnityEngine;

public class BushAI : MonoBehaviour
{
    public float moveSpeed = 2f;          // 行走速度
    public float attackRange = 1.5f;      // 攻击范围
    public float detectRange = 5f;        // 侦测范围
    public int maxHealth = 100;           // 最大血量
    private int currentHealth;

    private Transform player;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 距离检测
        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= detectRange)
        {
            WalkTowardsPlayer();
        }
        else
        {
            Idle();
        }
    }

    void WalkTowardsPlayer()
    {
        animator.Play("Walk");
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
    }

    void Attack()
    {
        animator.Play("Attack");
        // 在动画事件里触发伤害判定
    }

    void Idle()
    {
        animator.Play("Idle");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.Play("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.Play("Death");
        // 可加销毁逻辑 Destroy(gameObject, 2f);
    }
}

