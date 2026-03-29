using UnityEngine.EventSystems;

public class CraftSlot_UI : InventorySlot_UI
{
    private ItemData_Equipment equipment;

    protected override void Awake()
    {
        base.Awake();
    }

    //this function is to make the text lanaguge of this equipment's name
    //to get updated immediately after changing text language in options
    private void OnEnable()
    {
        SetupCraftSlot(equipment);
    }

    public void SetupCraftSlot(ItemData_Equipment _item)
    {
        if (_item == null)
        {
            return;
        }

        equipment = _item;

        inventorySlot.item = _item;
        itemImage.sprite = _item.icon;
        
        //english
        if (LanguageManager.instance.localeID == 0)
        {
            itemText.text = _item.itemName;
        }
        //chinese
        else if (LanguageManager.instance.localeID == 1)
        {
            itemText.text = _item.itemName_Chinese;
        }


        if (itemText.text.Length > 12)
        {
            itemText.fontSize = itemText.fontSize * 0.8f;
        }
        else
        {
            itemText.fontSize = 24;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(inventorySlot.item as ItemData_Equipment);
    }
}
