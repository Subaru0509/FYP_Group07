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
    // Start is called before the first frame update
    void Start()
    {
        max_hp = 20;
        hp = max_hp;
        status = Status.idle;
        if (this.transform.GetComponent<SpriteRenderer>().flipX)
        {
            face = Face.Left;
        }
        else
        {
            {
                face = Face.Right;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Destroy(this.gameObject);
        }
        //update status action;
        switch (status)
        {
            case Status.idle:
                break;
            case Status.run:

                break;
        }
    }
}
