using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public static BossRoom Instance;
    public Transform birthpoint;
    private Transform player;
    public GameObject wintext;
   

    public GameObject boss;

    private void Awake()
    {
        Instance=this;
    }
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        player.position = birthpoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(boss == null)
            wintext.SetActive(true);

    }

   
}
