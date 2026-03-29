using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventorySlot inventorySlot; //value is assigned in UpdateInventroySlotUI

    protected UI ui;

    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateInventorySlotUI(InventorySlot _inventorySlot)
    {
        inventorySlot = _inventorySlot;

        itemImage.color = Color.white;

        if (inventorySlot != null)
        {
            itemImage.sprite = inventorySlot.item.icon;

            if (inventorySlot.stackSize > 1)
            {
                itemText.text = inventorySlot.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpInventorySlotUI()
    {
        inventorySlot = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = null;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //when clikcing the invisible slot UI,
        //directly return to prevent bugs
        if (inventorySlot == null)
        {
            return;
        }

        //LCtrl + mouse_left to delete items from inventory
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(inventorySlot.item);
            return;
        }

        if (inventorySlot.item.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(inventorySlot.item);
        }

        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        if (inventorySlot.item.itemType == ItemType.Material)
        {
            return;
        }

        float yOffset = 0;

        ItemData_Equipment equipment = inventorySlot.item as ItemData_Equipment;

        //if the item slot is on the lower side of the screen,
        //item tooltip should be above the item
        if (transform.position.y <= Screen.height * 0.5)
        {
            if (equipment.GetItemStatInfoAndEffectDescription().Length >= 50)
            {
                yOffset = Screen.height * 0.01f + (equipment.GetItemStatInfoAndEffectDescription().Length - 50) * Screen.height * 0.001f;
            }
            else
            {
                yOffset = Screen.height * 0.01f;
            }
        }
        else //if the item slot is on the upper side of the screen, item tooltip should be below the item
        {
            yOffset = -Screen.height * 0.05f;
        }


        ui.itemToolTip.transform.position = new Vector2(transform.position.x - Screen.width * 0.13f, transform.position.y + yOffset);

        ui.itemToolTip.ShowToolTip(inventorySlot.item as ItemData_Equipment);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.item == null)
        {
            return;
        }

        ui.itemToolTip.HideToolTip();
    }
}
