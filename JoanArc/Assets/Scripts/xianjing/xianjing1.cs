using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xianjing1 : MonoBehaviour
{

    public bool candamage;
    private float timer;

    private Animator anim;

    private bool hasok;
    private void Start()
    {
        anim = GetComponent<Animator>();
        hasok = false;
    }
    private void Update()
    {
    
        timer += Time.deltaTime;

        if (timer > 3 && !hasok)
        {
            anim.SetTrigger("ok");
            candamage = true;
            hasok = true;
        }

        if(timer >4)
        {
            timer = 0;
            candamage = false;
            hasok = false;
        }
     


    
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (candamage && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().TakeDamage(50, collision.transform, collision.transform, false);
        }

    }
}
