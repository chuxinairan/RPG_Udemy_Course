using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;
    protected UI ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _item)
    {
        item = _item;

        itemIcon.color = Color.white;
        if(item!= null)
        {
            itemIcon.sprite = item.itemData.itemIcon;
            if(item.stackSize > 1)
            {
               itemText.text = item.stackSize.ToString();
            } else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemIcon.sprite = null;
        itemIcon.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.itemData);
            return;
        }

        if (item.itemData.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.itemData);

        ui.itemToolTip.HideToolTip();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.itemData.itemType == ItemType.Material)
            return;

        ui.itemToolTip.ShowToolTip(item.itemData as ItemData_Equipment);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.HideToolTip();
    }
}
