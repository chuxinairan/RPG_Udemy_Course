using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StatToolTip : UI_ToolTip
{
    [SerializeField] public TextMeshProUGUI description;

    public void ShowToolTip(string _text)
    {
        description.text = _text;

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
