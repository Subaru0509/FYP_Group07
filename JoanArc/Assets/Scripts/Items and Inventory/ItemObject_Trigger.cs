using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject;

    private void Awake()
    {
        myItemObject = GetComponentInParent<ItemObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.instance.player.stats.isDead)
        {
            return;
        }

        if (collision.GetComponent<Player>() != null)
        {
            //Debug.Log($"Picke up {myItemObject.gameObject.name}");
            myItemObject.PickupItem();
        }
    }
}
