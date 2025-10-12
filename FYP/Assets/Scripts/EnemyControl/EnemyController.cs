using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    int hp = 0;
    public int max_hp = 0;
    // Start is called before the first frame update
    void Start()
    {
        max_hp = 20;
        hp = max_hp;

    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
