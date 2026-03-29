using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flask_UI : MonoBehaviour
{
    public static Flask_UI instance;

    public Image flaskImage;
    public Image flaskCooldownImage;

    //private ItemData_Equipment flask;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        //at first if no flask is equipped there shouldn't be a flask icon in skill panel
        SetFlaskImage(Inventory.instance.GetEquippedEquipmentByType(EquipmentType.Flask));
    }

    //called in Inventory.EquipItem if the item to equip is a flask
    //also called in Inventroy.UnequipItem if there's no flask equipped
    public void SetFlaskImage(ItemData_Equipment _flask)
    {
        if (_flask == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            flaskImage.sprite = _flask.icon;
            flaskCooldownImage.sprite = _flask.icon;
        }
    }
}
