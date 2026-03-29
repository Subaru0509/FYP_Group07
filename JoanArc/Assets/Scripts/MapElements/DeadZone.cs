using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if entity falls down into the pit, die
        if (collision.GetComponent<CharacterStats>() != null)
        {
            collision.GetComponent<CharacterStats>()?.DieFromFalling();
        }
        else //if item falls down into the pit, destroy the item
        {
            Destroy(collision.gameObject);
        }
    }
}
