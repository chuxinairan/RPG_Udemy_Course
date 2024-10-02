using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] public TextMeshProUGUI skillName;
    [SerializeField] public TextMeshProUGUI skillDescription;
    [SerializeField] public TextMeshProUGUI skillPrice;

    public void ShowToolTip(string _name, string _description, string _price)
    {
        skillName.text = _name;
        skillDescription.text = _description;
        skillPrice.text = _price;

        AdjustPosition();
        AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;

        gameObject.SetActive(false);
    }
}
