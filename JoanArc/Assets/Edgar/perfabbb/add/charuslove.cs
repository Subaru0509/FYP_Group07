using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class charuslove : MonoBehaviour
{
    public static charuslove Instance;
    private void Awake()
    {
        Instance = this;
    }

    private GameObject aimed;
    private Transform tilemaps;
    private GameObject wall;

    private float addtime ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //addtime += Time.deltaTime;
    }

    public void solvecharu()
    {
        if (BossRoom.Instance != null)
        {
            aimed = GameObject.Find("Boss(Clone)");
            tilemaps = aimed.transform.Find("Tilemaps");
            wall = tilemaps.transform.Find("Walls").gameObject;

            CompositeCollider2D compCollider1 = wall.GetComponent<CompositeCollider2D>();

            if (compCollider1 != null)
            {
                // ����1��������������������ã�
                Destroy(compCollider1);

            }
            return;
        }

        aimed = GameObject.Find("Generated Level");
        tilemaps = aimed.transform.Find("Tilemaps");
        wall = tilemaps.transform.Find("Walls").gameObject;

        CompositeCollider2D compCollider = wall.GetComponent<CompositeCollider2D>();

        if (compCollider != null)
        {
            // ����1��������������������ã�
            Destroy(compCollider);

        }
    }

    public void  addcharu()
    {
        if (BossRoom.Instance != null)
        {
            aimed = GameObject.Find("Boss(Clone)");
            tilemaps = aimed.transform.Find("Tilemaps");
            wall = tilemaps.transform.Find("Walls").gameObject;

            CompositeCollider2D compCollider2 = wall.GetComponent<CompositeCollider2D>();

            if (compCollider2 == null)
            {
                wall.AddComponent<CompositeCollider2D>();

            }

            return;
        }
        aimed = GameObject.Find("Generated Level");
        tilemaps = aimed.transform.Find("Tilemaps");
        wall = tilemaps.transform.Find("Walls").gameObject;

        CompositeCollider2D compCollider = wall.GetComponent<CompositeCollider2D>();

        if (compCollider == null)
        {
            wall.AddComponent<CompositeCollider2D>();

        }

    }
}
