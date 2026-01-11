using UnityEngine;

public class BushAI : MonoBehaviour
{
    public float moveSpeed = 2f;         
    public float attackRange = 1.5f;      
    public float detectRange = 5f;        
    public int maxHealth = 100;           
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

        // Distance detection
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
        // Trigger damage judgment in animation events
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
        // ¿É¼ÓÏú»ÙÂß¼­ Destroy(gameObject, 2f);
    }
}

