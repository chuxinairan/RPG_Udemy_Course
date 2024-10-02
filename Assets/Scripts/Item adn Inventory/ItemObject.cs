using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    private Vector2 velocity;
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _data, Vector2 _velocity)
    {
        itemData = _data;
        velocity = _velocity;
        rb.velocity = velocity;
        SetupVisuals();
    }

    public void PickUpItem()
    {
        if (!Inventory.instance.CanAddItem(itemData) && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        Inventory.instance.AddItem(itemData);
        AudioManager.instance.PlaySFX(18, null);
        Destroy(gameObject);
    }
}
