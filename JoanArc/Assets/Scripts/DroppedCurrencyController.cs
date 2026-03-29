using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCurrencyController : MonoBehaviour
{
    public int droppedCurrency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency += droppedCurrency;
            Destroy(gameObject);
        }
    }
}
