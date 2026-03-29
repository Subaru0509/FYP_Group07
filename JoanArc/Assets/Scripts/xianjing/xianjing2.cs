using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xianjing2 : MonoBehaviour
{
    public bool ifmove;
    private Vector3 initposition;
    public bool ifright;

    private float timer;

    public float movespeed;
    private void Start()
    {
        initposition = transform.position;
    }
    private void Update()
    {
        if(ifmove)
        {
            timer += Time.deltaTime;

            if (timer < 5)
            {
                if (ifright) //陷阱向右运动
                {
                    transform.position += new Vector3(movespeed * Time.deltaTime, 0, 0);
                }
                else  //陷阱向左运动
                {
                    transform.position -= new Vector3(movespeed * Time.deltaTime, 0, 0);
                }
            }
            else if(timer <6)
            {

            }
            else
            {
                timer = 0;
                ifright = !ifright;
            }



        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>().TakeDamage(50, collision.transform, collision.transform, false);
        }

    }

}
