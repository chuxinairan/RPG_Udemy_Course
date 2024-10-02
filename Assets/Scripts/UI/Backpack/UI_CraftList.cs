using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private List<ItemData_Equipment> equipments;
    [SerializeField] private EquipmengType equipmentType;
    [SerializeField] private bool isInit;

    private void OnValidate()
    {
        gameObject.name = "CraftList - " + equipmentType.ToString();
    }

    void Start()
    {
        if (isInit)
            SetupCraftList();
    }

    private void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i=0; i< equipments.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(equipments[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}
