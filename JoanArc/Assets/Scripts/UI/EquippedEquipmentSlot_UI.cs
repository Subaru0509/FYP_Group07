using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedEquipmentSlot_UI : InventorySlot_UI
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //when clikcing the invisible slot UI,
        //directly return to prevent bugs

        //Sometimes Unity will have a bug
        //that even if there's no equipped equipments
        //unity will still think the inventorySlot of the first equipped equipment
        //is not null
        //if this bug occurs, restart unity
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        Inventory.instance.UnequipEquipmentWithoutAddingBackToInventory(inventorySlot.item as ItemData_Equipment);
        Inventory.instance.AddItem(inventorySlot.item as ItemData_Equipment);
        CleanUpInventorySlotUI();  //redundant cuz UpdateAllSlotUI() is already called in AddItem()

        ui.itemToolTip.HideToolTip();
    }
}
