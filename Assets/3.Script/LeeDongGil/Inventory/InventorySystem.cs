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
                if(slot.slotItemName.Equals(slot.transform.GetComponentInChildren<ItemInfo>()._itemName))
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

    public void SetItemInfo(ItemData _itemData, Slot _slot)
    {
        _slot.transform.GetChild(0).GetComponent<Image>().sprite = _itemData.sprite;
        _slot.transform.GetComponentInChildren<ItemInfo>()._itemName = _itemData.itemName;
    }
}
