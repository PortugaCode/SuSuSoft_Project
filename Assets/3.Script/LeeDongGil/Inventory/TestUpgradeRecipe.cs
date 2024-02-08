using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpgradeRecipe : MonoBehaviour
{
    public ItemInfo item;
    public InventorySystem invenSys;
    public ItemData tier2Item;
    public Recipe[] recipes;
    private void Awake()
    {
        invenSys = FindObjectOfType<InventorySystem>();
    }

    public void Upgrade()
    {
        Slot[] upSlots = GetComponentsInChildren<Slot>();
        foreach (Slot upSlot in upSlots)
        {
            if (upSlot.itemInfomation != null)
            {
                if (upSlot.slotItemCount >= 10 && upSlot.slotItemName.Equals("º¸¼®1"))
                {
                    int result = upSlot.slotItemCount / 10;
                    invenSys.GetItem(tier2Item, 1 * result);
                    upSlot.GetComponentInChildren<ItemInfo>()._itemCount -= result * 10;
                    break;
                }
            }
            else
            {
                continue;
            }
        }
    }
}
