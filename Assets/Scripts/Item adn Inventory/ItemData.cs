using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    [TextArea]
    public string itemEffectDescription;
    public string ItemID;

    public int dropChance;
    public float itemCooldown;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
        #if UNITY_EDITOR
        string itemPath = AssetDatabase.GetAssetPath(this);
        ItemID = AssetDatabase.AssetPathToGUID(itemPath);
        #endif
    }

    public virtual string GetDescription(bool _isCraft)
    {
        return "";
    }
}
