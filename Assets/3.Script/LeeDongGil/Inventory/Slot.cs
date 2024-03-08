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

    public int currentIndex = 0;

    private void Start()
    {
        if (itemData != null)
        {
            currentIndex = itemInfo._itemData.itemID;
        }
    }

}
