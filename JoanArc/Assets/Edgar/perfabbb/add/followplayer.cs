using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followplayer : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("updateposition", 1.1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void updateposition()
    {
        this.transform.position = player.transform.position;
    }
}
