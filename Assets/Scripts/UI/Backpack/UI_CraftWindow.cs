using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image[] materialIcons;
    [SerializeField] private Button craftButton;

    [SerializeField] private ItemData_Equipment initItem;

    private void Start()
    {
        SetupCraftWindow(initItem);
    }

    public void SetupCraftWindow(ItemData_Equipment _item)
    {
        craftButton.onClick.RemoveAllListeners();
        itemName.text = _item.itemName;
        itemDescription.text = _item.GetDescription(true);
        itemIcon.sprite = _item.itemIcon;

        if (_item.requiredMaterials.Count > materialIcons.Length)
            Debug.Log("You have more materils amount than you have material slots in craft!");

        for (int i=0; i<_item.requiredMaterials.Count; i++)
        {
            Image materialIcon = materialIcons[i].GetComponentsInChildren<Image>()[1];
            materialIcon.sprite = _item.requiredMaterials[i].itemData.itemIcon;
            materialIcon.color = Color.white;
            TextMeshProUGUI textGUI = materialIcons[i].GetComponentInChildren<TextMeshProUGUI>();
            textGUI.text = _item.requiredMaterials[i].stackSize.ToString();
        }

        for (int i = materialIcons.Length - 1; i >= _item.requiredMaterials.Count; i--)
        {
            Image materialIcon = materialIcons[i].GetComponentsInChildren<Image>()[1];
            materialIcon.sprite = null;
            materialIcon.color = Color.clear;
            materialIcons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_item, _item.requiredMaterials));
    }
}
