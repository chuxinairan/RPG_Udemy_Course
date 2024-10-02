using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    [TextArea]
    [SerializeField] private string statDesctiption;

    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        statNameText.text = statName;
    }
    private void Start()
    {
        ui = GetComponentInParent<UI>();
        UpdataStatUI();
    }
    public void UpdataStatUI()
    {
        PlayerStats stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        statValueText.text = stats.GetStatBy(statType).GetValue().ToString();

        if(statType == StatType.maxHealth)
            statValueText.text = stats.GetMaxHealthValue().ToString();

        if(statType == StatType.damage)
            statValueText.text = (stats.damage.GetValue() + stats.strength.GetValue()).ToString();

        if (statType == StatType.critPower)
            statValueText.text = (stats.critPower.GetValue() + stats.strength.GetValue()).ToString();

        if (statType == StatType.critChance)
            statValueText.text = (stats.critChance.GetValue() + stats.agility.GetValue()).ToString();

        if (statType == StatType.evasion)
            statValueText.text = (stats.evasion.GetValue() + stats.agility.GetValue()).ToString();

        if (statType == StatType.magicResistence)
            statValueText.text = (stats.magicResistence.GetValue() + stats.intelligence.GetValue() * 3).ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(statDesctiption);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideToolTip();
    }
}
