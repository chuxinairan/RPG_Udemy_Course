using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    public void SetupCraftSlot(ItemData_Equipment _item)
    {
        item.itemData = _item;

        itemIcon.sprite = _item.itemIcon;
        itemText.text = _item.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.itemData as ItemData_Equipment);
    }

}
