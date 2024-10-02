using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(ItemData_Equipment _item)
    {
        gameObject.SetActive(true);
        itemName.text = _item.itemName;
        itemType.text = _item.equipmengtType.ToString();
        itemDescription.text = _item.GetDescription(false);

        AdjustPosition();
        AdjustFontSize(itemName);
    }

    public void HideToolTip()
    {
        itemName.fontSize = defaultNameFontSize;

        gameObject.SetActive(false);
    }
}
