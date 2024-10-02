using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItem;

    public List<InventoryItem> equipment;
    private Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    private Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    private Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentItemSlots;
    private UI_StatSlot[] statSlots;

    [Header("Equipment effect cooldown")]
    private float lastUseFlaskTime;
    private float lastUseArmorTime;
    public float flaskCooldown;
    public float armorCooldown;

    [Header("Data base")]
    public List<InventoryItem> loadedItem;
    public List<ItemData_Equipment> loadedEquipment;
    private string[] assetNames;
    public List<ItemData> itemDataBase;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentItemSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItem();
    }

    private void AddStartingItem()
    {
        if(loadedEquipment.Count > 0)
        {
            foreach(ItemData_Equipment item in loadedEquipment)
            {
                EquipItem(item);
            }
        }

        if(loadedItem.Count > 0)
        {
            foreach(InventoryItem item in loadedItem)
            {
                for(int i=0; i < item.stackSize; i++)
                {
                    AddItem(item.itemData);
                }
            }
            return;
        }

        foreach(ItemData item in startingItem)
        {
            AddItem(item);
        }
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentItemSlots.Length; i++)
        {
            equipmentItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < equipmentItemSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmengtType == equipmentItemSlots[i].slotType)
                    equipmentItemSlots[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }

        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdataStatUI();
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment equipmentItem = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(equipmentItem);

        ItemData_Equipment oldEquipmeng = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmengtType == equipmentItem.equipmengtType)
                oldEquipmeng = item.Key;
        }

        if(oldEquipmeng != null)
        {
            UnequipItem(oldEquipmeng);
            AddItem(oldEquipmeng);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(equipmentItem, newItem);
        equipmentItem.AddModifiers();
        // ¸üÐÂmaxHealth
        PlayerManager.instance.player.GetComponent<PlayerStats>().onHealthChanged?.Invoke();
        RemoveItem(_item);
    }

    public void UnequipItem(ItemData_Equipment oldEquipmeng)
    {
        if (equipmentDictionary.TryGetValue(oldEquipmeng, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldEquipmeng);
            oldEquipmeng.RemoveModifiers();

            PlayerManager.instance.player.GetComponent<PlayerStats>().onHealthChanged?.Invoke();
        }
    }

    public void AddItem(ItemData _item)
    {
        if(CanAddItem(_item) && _item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();
    }

    public bool CanAddItem(ItemData _item)
    {
        if(_item != null)
            foreach(InventoryItem item in inventory)
            {
                if (item.itemData == _item)
                  return true;
            }

        if(inventory.Count >= inventoryItemSlots.Length)
        {
            Debug.Log("No more space!");
            return false;
        }
        return true;
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            RemoveFromInventory(_item);
        }
        else
        {
            RemoveFromStash(_item);
        }
        UpdateSlotUI();
    }

    private void RemoveFromStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                stash.Remove(value);
                stashDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }

    private void RemoveFromInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterial)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        foreach(InventoryItem item in _requiredMaterial)
        {
            if(stashDictionary.TryGetValue(item.itemData, out InventoryItem value))
            {
                if(value.stackSize < item.stackSize)
                {
                    Debug.Log("not enough Materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(item);
                }
            } else
            {
                Debug.Log("not enough Materials");
                return false;
            }
        }

        foreach(InventoryItem item in materialsToRemove)
        {
            for(int i = 0; i < item.stackSize; i++)
                RemoveItem(item.itemData);
        }

        AddItem(_itemToCraft);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;
    public ItemData_Equipment GetEquipment(EquipmengType _type)
    {
        ItemData_Equipment equipmentItem = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmengtType == _type)
                equipmentItem = item.Key;
        }
        return equipmentItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmengType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastUseFlaskTime + flaskCooldown;
        if(canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastUseFlaskTime = Time.time;
        } else
        {
            Debug.Log("Flask cooldown");
        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmengType.Armor);

        if (Time.time > lastUseArmorTime + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastUseArmorTime = Time.time;
            return true;
        }
        Debug.Log("Armor cooldown");
        return false;
    }

    void ISaveManager.LoadData(GameData _gameData)
    {
        foreach (KeyValuePair<string, int> pair in _gameData.inventory)
        {
            foreach(ItemData item in itemDataBase)
            {
                if(item != null && item.ItemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItem.Add(itemToLoad);
                }
            }
        }
        foreach (string id in _gameData.equipmentId)
        {
            foreach (ItemData item in itemDataBase)
            {
                if (item != null && item.ItemID == id)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        _gameData.inventory.Clear();
        _gameData.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _gameData.inventory.Add(pair.Key.ItemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _gameData.inventory.Add(pair.Key.ItemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _gameData.equipmentId.Add(pair.Key.ItemID);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item database")]
    private void FillUpItemDatabase() => itemDataBase = GetItemDataBase();

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });
        foreach(string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
#endif
}
