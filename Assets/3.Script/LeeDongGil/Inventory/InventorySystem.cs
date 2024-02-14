using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public void GetItem(ItemData itemData, int getCount)
    {
        Slot[] slots = GetComponentsInChildren<Slot>();
        foreach(Slot slot in slots)
        {
            if(slot.IsSlotUse)
            {
                if(slot.slotItemName.Equals(itemData.itemName))
                {
                    slot.GetComponentInChildren<ItemInfo>()._itemCount += getCount;
                    break;
                }
                else continue;
            }
            else
            {
                SetItemInfo(itemData, slot);
                slot.GetComponentInChildren<ItemInfo>()._itemCount = getCount;
                break;
            }
        }
    }

    public void GetHousingItem_test(HousingItemData housingItemData, int getCount)
    {
        HousingSlot[] slots = GetComponentsInChildren<HousingSlot>();
        foreach (HousingSlot slot in slots)
        {
            if (slot.IsSlotUse)
            {
                if (slot.slotItemName.Equals(housingItemData.housingENName))
                {
                    slot.GetComponentInChildren<HousingInventory>().count += getCount;
                    break;
                }
                else continue;
            }
            else
            {
                SetItemInfo(housingItemData, slot);
                slot.GetComponentInChildren<HousingInventory>().count = getCount;
                break;
            }
        }
    }

    public void SetItemInfo(ItemData itemData, Slot _slot)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = itemData.sprite;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemData = itemData;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemName = itemData.itemName;
    }

    public void SetItemInfo(HousingItemData housingData, HousingSlot _slot)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = housingData.housingSprite;
        _slot.transform.GetComponentInChildren<HousingInventory>().housingData = housingData;
        _slot.transform.GetComponentInChildren<HousingInventory>().housingName = housingData.housingENName;
    }
}
