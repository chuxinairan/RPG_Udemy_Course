using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private UI_SkillTreeSlot[] shouldToUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldToLocked;
    [SerializeField] private Color lockedColor;

    private UI ui;
    private Image image;

    public bool unlocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    void Start()
    {
        ui = GetComponentInParent<UI>();
        image = GetComponent<Image>();
        image.color = lockedColor;
        if(unlocked)
            image.color = Color.white;
    }

    private void UnlockSkillSlot()
    {
        if (!PlayerManager.instance.HaveEnoughMoney(skillPrice))
            return;

        for(int i=0; i<shouldToUnlocked.Length; i++)
        {
            if (shouldToUnlocked[i].unlocked == false)
            {
                Debug.Log("has unlocked prerequisite skill!");
                return;
            }
        }

        for (int i = 0; i <shouldToLocked.Length; i++)
        {
            if (shouldToLocked[i].unlocked == true)
            {
                Debug.Log("conflict to other skill!");
                return;
            }
        }

        unlocked = true;
        image.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName, skillDescription, skillPrice.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    void ISaveManager.LoadData(GameData _gameData)
    {
        if (_gameData.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    void ISaveManager.SaveData(ref GameData _gameData)
    {
        if (_gameData.skillTree.TryGetValue(skillName, out bool value))
        {
            _gameData.skillTree.Remove(skillName);
            _gameData.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _gameData.skillTree.Add(skillName, unlocked);
        }
    }
}
