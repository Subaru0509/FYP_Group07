using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();

    [Header("Chest Settings")]
    [SerializeField] private Vector2 knockback;

    [Header("Potion Settings")]
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Transform spawnPoint;

    private bool isOpened = false;

    public void TakeDamage(float damage, Transform damageDealer)
    {
        if (isOpened) return; 

        isOpened = true;
        fx.PlayOnDamageVfx();
        anim.SetBool("chestOpen", true);
        rb.velocity = knockback;
        rb.angularVelocity = Random.Range(-200f, 200f);

        Instantiate(potionPrefab, spawnPoint.position, Quaternion.identity);
    }
}
