using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, IGameProgressionSaveManager
{
    public static Inventory instance;

    //inventory stores equipments
    public List<InventorySlot> inventorySlotList;
    public Dictionary<ItemData, InventorySlot> inventorySlotDictionary;

    //stash stores materials
    public List<InventorySlot> stashSlotList;
    public Dictionary<ItemData, InventorySlot> stashSlotDictionary;

    //equipped equipments
    public List<InventorySlot> equippedEquipmentSlotList;
    public Dictionary<ItemData_Equipment, InventorySlot> equippedEquipmentSlotDictionary;


    public List<ItemData> startItems;

    [Header("Inventory UI")]
    [SerializeField] private Transform referenceInventory;
    [SerializeField] private Transform referenceStash;
    [SerializeField] private Transform referenceEquippedEquipments;
    [SerializeField] private Transform referenceStatPanel;

    private InventorySlot_UI[] inventorySlotUI;
    private InventorySlot_UI[] stashSlotUI;
    private EquippedEquipmentSlot_UI[] equippedEquipmentSlotUI;
    private StatSlot_UI[] statSlotUI;

    //private float flaskLastUseTime;
    //private bool flaskUsed = false;


    [Header("Item Data Base")]
    public List<ItemData> itemDataBase;
    public List<InventorySlot> loadedInventorySlots;
    public List<ItemData_Equipment> loadedEquippedEquipment;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventorySlotList = new List<InventorySlot>();
        inventorySlotDictionary = new Dictionary<ItemData, InventorySlot>();

        stashSlotList = new List<InventorySlot>();
        stashSlotDictionary = new Dictionary<ItemData, InventorySlot>();

        equippedEquipmentSlotList = new List<InventorySlot>();
        equippedEquipmentSlotDictionary = new Dictionary<ItemData_Equipment, InventorySlot>();

        inventorySlotUI = referenceInventory.GetComponentsInChildren<InventorySlot_UI>();
        stashSlotUI = referenceStash.GetComponentsInChildren<InventorySlot_UI>();
        equippedEquipmentSlotUI = referenceEquippedEquipments.GetComponentsInChildren<EquippedEquipmentSlot_UI>();
        statSlotUI = referenceStatPanel.GetComponentsInChildren<StatSlot_UI>();

        AddStartItems();
        RefreshEquipmentEffectUseState();
    }

    private void AddStartItems()
    {
        foreach (var equipment in loadedEquippedEquipment)
        {
            EquipItem(equipment);
        }

        if (loadedInventorySlots.Count > 0)
        {
            foreach (var slot in loadedInventorySlots)
            {
                for (int i = 0; i < slot.stackSize; i++)
                {
                    AddItem(slot.item);
                }
            }

            return;
        }

        for (int i = 0; i < startItems.Count; i++)
        {
            if (startItems[i] != null)
            {
                AddItem(startItems[i]);
            }
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ItemData _itemToRemove = inventorySlotList[inventorySlotList.Count - 1].item;
        //    RemoveItem(_itemToRemove);
        //}
    }

    private void UpdateAllSlotUI()
    {
        //Clean up all the slot UIs before update
        //to ensure no extra slot UI exists after update
        for (int i = 0; i < inventorySlotUI.Length; i++)
        {
            inventorySlotUI[i].CleanUpInventorySlotUI();
        }

        for (int i = 0; i < stashSlotUI.Length; i++)
        {
            stashSlotUI[i].CleanUpInventorySlotUI();
        }

        for (int i = 0; i < equippedEquipmentSlotUI.Length; i++)
        {
            equippedEquipmentSlotUI[i].CleanUpInventorySlotUI();
        }


        for (int i = 0; i < inventorySlotList.Count; i++)
        {
            inventorySlotUI[i].UpdateInventorySlotUI(inventorySlotList[i]);
        }

        for (int i = 0; i < stashSlotList.Count; i++)
        {
            stashSlotUI[i].UpdateInventorySlotUI(stashSlotList[i]);
        }

        //update equipped equipments UI when equipping equipments
        for (int i = 0; i < equippedEquipmentSlotUI.Length; i++)
        {
            //if in the equipped equipment list there's an equipment
            //whose type is same as this UI slot equipment type
            //(e.g. the equipped equipment is a weapon, and this is a weapon slot UI)
            //update this UI slot according to that equipment
            foreach (var search in equippedEquipmentSlotDictionary)
            {
                if (search.Key.equipmentType == equippedEquipmentSlotUI[i].equipmentType)
                {
                    equippedEquipmentSlotUI[i].UpdateInventorySlotUI(search.Value);
                }
            }

        }

        //update Stat Value in stat panel (character UI)
        UpdateStatUI();

    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlotUI.Length; i++)
        {
            statSlotUI[i].UpdateStatValue_UI();
        }
    }

    public void EquipItem(ItemData _item)
    {
        //convert ItemData type to ItemData_Equipment type (father class -> child class)
        ItemData_Equipment _newEquipmentToEquip = _item as ItemData_Equipment;

        InventorySlot newEquipmentSlot = new InventorySlot(_newEquipmentToEquip);

        //if this type of equipment is already equipped,
        //remove the equipped one to equip the new one
        ItemData_Equipment _oldEquippedEquipment = null;

        //var search here is same as KeyValuePair<ItemData_Equipment, InventorySlot>
        foreach (var search in equippedEquipmentSlotDictionary)
        {
            if (search.Key.equipmentType == _newEquipmentToEquip.equipmentType)
            {
                _oldEquippedEquipment = search.Key;
            }
        }

        if (_oldEquippedEquipment != null)
        {
            UnequipEquipmentWithoutAddingBackToInventory(_oldEquippedEquipment);

            //the unequipped old equipment will get back to inventory
            AddItem(_oldEquippedEquipment);
        }

        equippedEquipmentSlotList.Add(newEquipmentSlot);
        equippedEquipmentSlotDictionary.Add(_newEquipmentToEquip, newEquipmentSlot);
        _newEquipmentToEquip.AddModifiers();

        //equipped equipment will be removed from inventory
        RemoveItem(_newEquipmentToEquip);
        //UpdateInventoryAndStashUI();

        if (_newEquipmentToEquip.equipmentType == EquipmentType.Flask)
        {
            Flask_UI.instance.SetFlaskImage(_newEquipmentToEquip);
        }
    }

    //Unequip here will not add the equipment back to inventory
    public void UnequipEquipmentWithoutAddingBackToInventory(ItemData_Equipment _equipmentToRemove)
    {
        if (equippedEquipmentSlotDictionary.TryGetValue(_equipmentToRemove, out InventorySlot value))
        {
            equippedEquipmentSlotList.Remove(value);
            equippedEquipmentSlotDictionary.Remove(_equipmentToRemove);
            _equipmentToRemove.RemoveModifiers();
        }

        if (_equipmentToRemove.equipmentType == EquipmentType.Flask)
        {
            Flask_UI.instance.SetFlaskImage(null);
        }
    }

    public bool CanAddEquipmentToInventory()
    {
        if (inventorySlotList.Count >= inventorySlotUI.Length)
        {
            Debug.Log("No more space in inventory");
            return false;
        }

        return true;
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddEquipmentToInventory())
        {
            AddEquipmentToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddMaterialToStash(_item);
        }

        UpdateAllSlotUI();
    }

    private void AddMaterialToStash(ItemData _item)
    {
        if (stashSlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            value.AddStack();
        }
        else
        {
            InventorySlot newItem = new InventorySlot(_item);
            stashSlotList.Add(newItem);
            stashSlotDictionary.Add(_item, newItem);
        }
    }

    private void AddEquipmentToInventory(ItemData _item)
    {
        //if this item is already in inventory, its stack size++
        if (inventorySlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            value.AddStack();
        }
        else  //if this item is not in inventory, add it to the inventoryItem list
        {
            InventorySlot newItem = new InventorySlot(_item);  //initialize the inventroyItem using contructor and make its stackSize++ (=1)
            inventorySlotList.Add(newItem);  //add this inventoryItem to the inventory item list
            inventorySlotDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventorySlotDictionary.TryGetValue(_item, out InventorySlot value))
        {
            //if there's only 1 or less amount of this item in inventory,
            //remove it from the inventory item list
            if (value.stackSize <= 1)
            {
                inventorySlotList.Remove(value);
                inventorySlotDictionary.Remove(_item);
            }
            else  //if there're multiple this items in inventory, the stack size--
            {
                value.RemoveStack();
            }
        }

        //For removing materials from stash
        if (stashSlotDictionary.TryGetValue(_item, out InventorySlot stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stashSlotList.Remove(stashValue);
                stashSlotDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }

        }


        UpdateAllSlotUI();
    }

    public List<InventorySlot> GetEquippedEquipmentList()
    {
        return equippedEquipmentSlotList;
    }

    public List<InventorySlot> GetStashList()
    {
        return stashSlotList;
    }

    public ItemData_Equipment GetEquippedEquipmentByType(EquipmentType _type)
    {
        ItemData_Equipment equippedEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventorySlot> search in equippedEquipmentSlotDictionary)
        {
            if (search.Key.equipmentType == _type)
            {
                equippedEquipment = search.Key;
            }
        }

        return equippedEquipment;
    }

    public bool CraftIfAvailable(ItemData_Equipment _equipmentToCraft, List<InventorySlot> _requiredMaterials)
    {
        List<InventorySlot> materialsToRemove = new List<InventorySlot>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            //if successfully find materials needed to craft
            if (stashSlotDictionary.TryGetValue(_requiredMaterials[i].item, out InventorySlot stashValue))
            {
                //if the amount of the material in stash is not enough
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("Not enough materials");
                    return false;
                }
                else  //if the amount of the material is enough in stash
                {
                    //add the required materials to the remove list
                    for (int k = 0; k < _requiredMaterials[i].stackSize; k++)
                    {
                        materialsToRemove.Add(stashValue);
                    }
                }

            }
            else  //if can't find required materials in stash
            {
                Debug.Log("Not enough materials");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].item);
        }

        AddItem(_equipmentToCraft);
        Debug.Log($"Crafted {_equipmentToCraft.itemName}");
        return true;
    }

    //public void UseFlask()
    //{
    //    ItemData_Equipment flask = GetEquippedEquipmentByType(EquipmentType.Flask);
    //    if (flask == null)
    //    {
    //        return;
    //    }

    //    bool canUseFlask = Time.time > flaskLastUseTime + flask.itemCooldown;

    //    if (canUseFlask || !flaskUsed)
    //    {
    //        flask.ExecuteItemEffect(null);
    //        flaskUsed = true;
    //        flaskLastUseTime = Time.time;
    //        Debug.Log("Use Flask");
    //    }
    //    else
    //    {
    //        Debug.Log("Flask is in cooldown");
    //    }
    //}

    public void UseFlask_ConsiderCooldown(Transform _spawnTransform)
    {
        ItemData_Equipment flask = GetEquippedEquipmentByType(EquipmentType.Flask);
        if (flask == null)
        {
            return;
        }

        flask.ExecuteItemEffect_ConsiderCooldown(_spawnTransform);
    }

    public void UseArmorEffect_ConsiderCooldown(Transform _spawnTransform)
    {
        ItemData_Equipment armor = GetEquippedEquipmentByType(EquipmentType.Armor);
        if (armor == null)
        {
            return;
        }

        armor.ExecuteItemEffect_ConsiderCooldown(_spawnTransform);
    }

    public void UseSwordEffect_ConsiderCooldown(Transform _spawnTransform)
    {
        ItemData_Equipment sword = GetEquippedEquipmentByType(EquipmentType.Weapon);
        if (sword == null)
        {
            return;
        }

        sword.ExecuteItemEffect_ConsiderCooldown(_spawnTransform);
    }

    public void ReleaseSwordArcane_ConsiderCooldown()
    {
        ItemData_Equipment sword = GetEquippedEquipmentByType(EquipmentType.Weapon);
        if (sword == null)
        {
            return;
        }

        sword.ReleaseSwordArcane_ConsiderCooldown();
    }

    public void UseCharmEffect_ConsiderCooldown(Transform _spawnTransform)
    {
        ItemData_Equipment charm = GetEquippedEquipmentByType(EquipmentType.Charm);
        if (charm == null)
        {
            return;
        }

        charm.ExecuteItemEffect_ConsiderCooldown(_spawnTransform);
    }

    private void RefreshEquipmentEffectUseState()
    {
        foreach (var search in inventorySlotDictionary)
        {
            var equipment = search.Key as ItemData_Equipment;
            equipment.RefreshUseState();
        }

        foreach (var search in equippedEquipmentSlotDictionary)
        {
            var equipment = search.Key as ItemData_Equipment;
            equipment.RefreshUseState();
        }
    }

    public void LoadData(GameData _data)
    {
        //inventory<itemID, stackSize>
        foreach (var pair in _data.inventory)
        {
            //ItemData item
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventorySlot slotToLoad = new InventorySlot(item);
                    slotToLoad.stackSize = pair.Value;

                    loadedInventorySlots.Add(slotToLoad);
                }
            }
        }

        foreach (var equipmentID in _data.equippedEquipmentIDs)
        {
            foreach (var equipment in itemDataBase)
            {
                if (equipment != null && equipment.itemID == equipmentID)
                {
                    loadedEquippedEquipment.Add(equipment as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        //prevent from having more and more items every time starting the game
        _data.inventory.Clear();
        _data.equippedEquipmentIDs.Clear();

        foreach (KeyValuePair<ItemData, InventorySlot> search in inventorySlotDictionary)
        {
            _data.inventory.Add(search.Key.itemID, search.Value.stackSize);
        }

        foreach (var search in stashSlotDictionary)
        {
            _data.inventory.Add(search.Key.itemID, search.Value.stackSize);
        }

        foreach (var search in equippedEquipmentSlotDictionary)
        {
            _data.equippedEquipmentIDs.Add(search.Key.itemID);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase()
    {
        itemDataBase = GetItemDataBase();
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();

        //find all the asset names in the specified path
        //here, new string[] {...} is same as new[] {...}, 
        //the latter is the Inplicit (unvisiable) type convert,
        //will judge the array type according to its content
        string[] assetNames = AssetDatabase.FindAssets("", new string[] { "Assets/ItemData/Items" });

        //SO means scriptable object
        foreach (string SOName in assetNames)
        {
            //get path to the item
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            //load the item in this path
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            //add this item to itemDataBase
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif

}
