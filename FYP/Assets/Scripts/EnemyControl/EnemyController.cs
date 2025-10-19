using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    int hp = 0;
    public int max_hp = 0;
    public enum Status{ idle,run,Attack,}
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
        status = Status.run;
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
                break;
        }
    }
}
