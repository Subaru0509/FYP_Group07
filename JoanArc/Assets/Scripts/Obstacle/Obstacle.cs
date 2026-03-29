using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected Collider2D cd;
    protected Rigidbody2D rb;
    public CharacterStats stats { get; private set; }

    private void Awake()
    {
        cd = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
    }
}
