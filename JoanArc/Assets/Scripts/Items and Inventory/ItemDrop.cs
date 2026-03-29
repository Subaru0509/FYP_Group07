using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxItemDropAmount;  //this enemy can drop how many items at most
    [SerializeField] private ItemData[] possibleDropItemList;  //the items that can be dropped by this enemy
    private List<ItemData> actualDropList = new List<ItemData>(); //the items that are actually dropped by this enemy

    [SerializeField] private GameObject dropItemPrefab;  //an empty prefab which can be setup to any itmes by SetupItemDrop() in ItemObject


    public virtual void GenrateDrop()
    {
        //add items to actualDropList by their chances
        for (int i = 0; i < possibleDropItemList.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDropItemList[i].dropChance)
            {
                actualDropList.Add(possibleDropItemList[i]);
            }
        }

        //drop items and delete them from actualDropList
        for (int i = 0; i < maxItemDropAmount && actualDropList.Count > 0; i++)
        {
            ItemData itemToDrop = actualDropList[Random.Range(0, actualDropList.Count - 1)];

            actualDropList.Remove(itemToDrop);
            DropItem(itemToDrop);
        }

    }


    //DropItem is called when Enemy dies
    protected void DropItem(ItemData _itemToDrop)
    {
        GameObject newDropItem = Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

        Vector2 dropVelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

        //this will together setup the drop item's name and icon
        newDropItem.GetComponent<ItemObject>()?.SetupItemDrop(_itemToDrop, dropVelocity);
    }
}
