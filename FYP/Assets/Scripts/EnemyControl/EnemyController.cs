using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    int hp = 0;
    public int max_hp = 0;
    public enum Status{ idle,run,ready,attack,}
    public Status status;
    public enum Face { Right,Left };
    public Face face;

    public float speed;
    private Transform myTransform;

    public Transform playerTransform;
    private SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        max_hp = 20;
        hp = max_hp;
        status = Status.idle;
        spr = this.transform.GetComponent<SpriteRenderer>();
        if (spr.flipX)
        {
            face = Face.Left;
        }
        else
        {
            
                face = Face.Right;
            
        }
        myTransform = this.transform;
        if (GameObject.Find("Player") != null)
        {
            playerTransform = GameObject.Find("Player").transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Destroy(this.gameObject);
        }

        float deltaTime = Time.deltaTime;
        //update status action;
        switch (status)
        {
            case Status.idle:
                if (playerTransform)
                {
                    if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) > 3f && Mathf.Abs(myTransform.position.x - playerTransform.position.x) < 6f)
                    {
                        status = Status.run;
                    }
                    else if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) > 1f && Mathf.Abs(myTransform.position.x - playerTransform.position.x) < 3f)
                    {
                        status = Status.attack;
                    }


                }
               
                break;
            

            case Status.run:
                if (playerTransform)
                {
                    if (myTransform.position.x >= playerTransform.position.x)
                {
                        spr.flipX = true;
                        face = Face.Right;
                }
                else
                {
                        spr.flipX = false;
                        face = Face.Left;
                }

                }
                
            

                switch (face)
                {
                    case Face.Left:
                        myTransform.position += new Vector3(speed * deltaTime, 0, 0);
                        break;
                    case Face.Right:
                        myTransform.position -= new Vector3(speed * deltaTime, 0, 0);
                        break;
                }
                if (playerTransform)
                {
                    if (Mathf.Abs(myTransform.position.x - playerTransform.position.x) >=6f)
                    {
                        status = Status.idle;
                    }
                } 
               
                break;
        }
    }
}
