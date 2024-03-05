using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public bool isUpgradeSlot = false;

    public bool isSlotUse = false;
    public Transform UpgradeSlot;
    public Transform InventorySlot;
    public InventorySystem invenSys;
    public string slotItemName = string.Empty;
    public int slotItemCount = 0;
    public ItemData itemData;
    public ItemInfo itemInfo;




    private void Update()
    {
        if(itemInfo._itemCount > 0)
        {
            isSlotUse = true;
        }
        else
        {
            isSlotUse = false;
        }

        if (isSlotUse)
        {
            slotItemName = itemInfo._itemName;
            slotItemCount = itemInfo._itemCount;
            itemData = itemInfo._itemData;
        }
        else
        {
            slotItemName = string.Empty;
            itemData = null;
        }
    }



}
