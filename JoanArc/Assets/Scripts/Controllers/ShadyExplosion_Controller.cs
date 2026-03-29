using UnityEngine;

public class ShadyExplosion_Controller : MonoBehaviour
{
    private Animator anim;
    private CharacterStats shadyStats;
    private float growSpeed;
    private float maxSize;
    private float explosionRadius;

    private bool canGrow = true;

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (maxSize - transform.localScale.x < 0.5f)
        {
            canGrow = false;
            anim.SetTrigger("Explosion");
        }
    }

    public void SetupExplosion(CharacterStats _shadyStats, float _growSpeed, float _maxSize, float _explosionRadius)
    {
        anim = GetComponent<Animator>();

        shadyStats = _shadyStats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explosionRadius = _explosionRadius;
    }


    private void Explosion()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                shadyStats.DoDamge(hit.GetComponent<PlayerStats>());
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.gameObject == gameObject)
                {
                    continue;
                }

                shadyStats.DoDamge(hit.GetComponent<EnemyStats>());
            }
        }
    }

    public void ShadyChangeToDeathState()
    {
        shadyStats.GetComponent<Shady>()?.Die();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
