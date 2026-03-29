using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] private float chanceToDropEquiment;
    [SerializeField] private float chanceToDropMaterials;

    public override void GenrateDrop()
    {
        //get list of equipped equipment
        //for each of them, drop them by their drop chances

        Inventory inventory = Inventory.instance;
        List<InventorySlot> currentEquippedEquipment = inventory.GetEquippedEquipmentList();
        List<InventorySlot> EquipmentToLose = new List<InventorySlot>();

        List<InventorySlot> currentStash = inventory.GetStashList();
        List<InventorySlot> materialsToLose = new List<InventorySlot>();

        for (int i = 0; i < currentEquippedEquipment.Count; i++)
        {
            if (Random.Range(0, 100) <= chanceToDropEquiment)
            {
                EquipmentToLose.Add(currentEquippedEquipment[i]);
            }
        }

        for (int i = 0; i < currentStash.Count; i++)
        {
            if (Random.Range(0, 100) <= chanceToDropMaterials)
            {
                materialsToLose.Add(currentStash[i]);
            }
        }

        for (int i = 0; i < EquipmentToLose.Count; i++)
        {
            inventory.UnequipEquipmentWithoutAddingBackToInventory(EquipmentToLose[i].item as ItemData_Equipment);
            DropItem(EquipmentToLose[i].item);
        }

        for (int i = 0; i < materialsToLose.Count; i++)
        {
            DropItem(materialsToLose[i].item);
            inventory.RemoveItem(materialsToLose[i].item);
        }
    }
}
