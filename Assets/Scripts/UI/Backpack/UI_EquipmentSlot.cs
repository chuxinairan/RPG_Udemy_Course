using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmengType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();    
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
        Inventory.instance.AddItem(item.itemData);
        CleanUpSlot();

        ui.itemToolTip.HideToolTip();
    }
}
