using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0.6f;
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    public Transform attackCheck;
    public float attackCheckRadius = 1.2f;

    [Header("Knockback Info")]
    public Vector2 knockbackMovement = new Vector2(5, 3);
    public Vector2 randomKnockbackMovementOffsetRange;
    [SerializeField] protected float knockbackDuration = 0.2f;
    public bool isKnockbacked { get; set; }

    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;

    #region components
    public SpriteRenderer sr { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    //public EntityFX fx { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    public System.Action onFlipped;

    protected virtual void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        //fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {

    }

    public virtual void DamageFlashEffect()
    {
        //fx.StartCoroutine("FlashFX");

    }

    public virtual void DamageKnockbackEffect(Transform _attacker, Transform _attackee)
    {
        float _knockbackDirection = CalculateKnockbackDirection(_attacker, _attackee);

        StartCoroutine(HitKnockback(_knockbackDirection));
    }


    protected virtual IEnumerator HitKnockback(float _knockbackDirection)
    {
        isKnockbacked = true;

        float xOffset = Random.Range(0, randomKnockbackMovementOffsetRange.x);
        float yOffset = Random.Range(0, randomKnockbackMovementOffsetRange.y);

        rb.velocity = new Vector2((knockbackMovement.x + xOffset) * _knockbackDirection, knockbackMovement.y + yOffset);
        //yield return new WaitForSeconds(0.1f);
        //rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = new Vector2(0, rb.velocity.y);

        isKnockbacked = false;
    }

    public virtual float CalculateKnockbackDirection(Transform _attacker, Transform _attackee)
    {
        float _knockbackDirection = 0;

        if (_attacker.position.x < _attackee.position.x)
        {
            _knockbackDirection = 1;
        }
        else if (_attacker.position.x > _attackee.position.x)
        {
            _knockbackDirection = -1;
        }

        return _knockbackDirection;
    }

    public virtual void SetupKnockbackMovement(Vector2 _knockbackMovement)
    {
        knockbackMovement = _knockbackMovement;
    }

    public virtual void SetupZeroKnockbackMovement()
    {

    }

    #region Velocity
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnockbacked)
        {
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public virtual void SetZeroVelocity()
    {
        if (isKnockbacked)
        {
            return;
        }

        rb.velocity = new Vector2(0, 0);
    }
    #endregion

    #region Collision
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual bool IsGroundDetected()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    public virtual bool IsWallDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDirection = -facingDirection;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
        {
            onFlipped();
        }
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void SetupDefaultFacingDirection(int _facingDirection)
    {
        facingDirection = _facingDirection;

        if (facingDirection == -1)
        {
            facingRight = false;
        }
    }
    #endregion



    public virtual void Die()
    {
        
    }

    public virtual void SlowSpeedBy(float _percentage, float _duration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

}
