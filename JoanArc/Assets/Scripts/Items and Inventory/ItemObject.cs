using UnityEngine;


//Remember to use Fill up item data base function in inventorymanager script in unity editor every time making a new item!
public class ItemObject : MapElement
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData item;

    protected override void Start()
    {
        SetupItemIconAndName();
        base.Start();
    }

    public void SetupItemDrop(ItemData _item, Vector2 _dropVelocity)
    {
        item = _item;
        rb.velocity = _dropVelocity;

        SetupItemIconAndName();
    }

    //pickup item is called in ItemObject_Trigger
    public void PickupItem()
    {
        if (!Inventory.instance.CanAddEquipmentToInventory() && item.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 5);

            //english
            if (LanguageManager.instance.localeID == 0)
            {
                PlayerManager.instance.player.fx.CreatePopUpText("No more space in inventory!");
            }
            //chinese
            else if (LanguageManager.instance.localeID == 1)
            {
                PlayerManager.instance.player.fx.CreatePopUpText("No more space in inventory!");
            }
            return;
        }

        Inventory.instance.AddItem(item);
        AudioManager.instance.PlaySFX(18, transform);

        GameManager.instance.UsedMapElementIDList.Add(mapElementID);

        Debug.Log($"Picked up item {item.itemName}");
        Destroy(gameObject);
    }

    private void SetupItemIconAndName()
    {
        if (item == null)
        {
            return;
        }

        GetComponent<SpriteRenderer>().sprite = item.icon;
        gameObject.name = $"Item Object - {item.name}";
    }
}
