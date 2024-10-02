using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLooseItems;

    public override void GenerateDrops()
    {
        Inventory instance = Inventory.instance;

        List<InventoryItem> equipment = instance.GetEquipmentList();
        List<InventoryItem> stash = instance.GetStashList();

        for (int i=0; i<equipment.Count; i++)
        {
            if(Random.Range(0,100) < chanceToLooseItems)
            {
                DropItem(equipment[i].itemData);
                instance.UnequipItem(equipment[i].itemData as ItemData_Equipment);
            }
        }

        for (int i = 0; i < stash.Count; i++)
        {
            if (Random.Range(0, 100) < chanceToLooseItems)
            {
                DropItem(stash[i].itemData);
                instance.RemoveItem(stash[i].itemData);
            }
        }
    }
}
