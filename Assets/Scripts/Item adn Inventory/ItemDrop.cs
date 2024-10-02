using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfDrop;
    [SerializeField] private ItemData[] possibleItem;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData itemData;

    public virtual void GenerateDrops()
    {
        foreach(ItemData item in possibleItem)
        {
            if (Random.Range(0, 100) < item.dropChance)
                dropList.Add(item);
        }

        for(int i=0; i < amountOfDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    protected void DropItem(ItemData _item)
    {
        GameObject itemObject = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-7, 7), Random.Range(15, 20));
        itemObject.GetComponent<ItemObject>().SetupItem(_item, randomVelocity);
    }
}
